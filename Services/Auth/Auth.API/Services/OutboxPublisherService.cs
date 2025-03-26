using System.Text.Json;
using Auth.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Services;

public class OutboxPublisherService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AuthContext>();
            
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            var messages = await db.OutboxMessages
                .Where(x => !x.IsPublished)
                .OrderBy(x => x.CreatedAt)
                .Take(20)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
            {
                var eventType = Type.GetType(message.Type);
                if (eventType == null) continue;

                var evt = JsonSerializer.Deserialize(message.Payload, eventType);
                if (evt == null) continue;

                await publishEndpoint.Publish(evt, cancellationToken);

                message.IsPublished = true;
                message.PublishedAt = DateTime.UtcNow;
            }

            await db.SaveChangesAsync(cancellationToken);
            await Task.Delay(3000, cancellationToken);
        }
    }
}