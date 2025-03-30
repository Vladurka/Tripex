using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.Outbox;
using Carter;
using Profiles.Infrastructure.Data;

namespace Profiles.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks();
        services.AddOutboxPattern<ProfilesContext>();
            
        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();
        app.UseExceptionHandler(opts => { });
        app.UseHealthChecks("/health");
            
        return app;
    }
}