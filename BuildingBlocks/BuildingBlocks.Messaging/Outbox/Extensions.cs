using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.Outbox;

public static class Extensions
{
    public static IServiceCollection AddOutboxPattern<T>(this IServiceCollection services) where T : DbContext, IOutboxContext
    {
        services.AddScoped(typeof(IOutboxRepository), typeof(OutboxRepository<T>));
        services.AddHostedService<OutboxPublisherService<T>>();

        return services;
    }
}