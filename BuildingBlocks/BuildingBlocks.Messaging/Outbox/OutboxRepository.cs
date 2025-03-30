using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Messaging.Outbox;

public class OutboxRepository<T>(T context) : IOutboxRepository where T : DbContext, IOutboxContext
{
    public async Task AddOutboxMessageAsync(OutboxMessage message)
    {
        await context.OutboxMessages.AddAsync(message);
        await context.SaveChangesAsync();
    }
}