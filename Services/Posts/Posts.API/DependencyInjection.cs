using BuildingBlocks.Auth;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.Outbox;
using Posts.Infrastructure.Data;

namespace Posts.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks();

        services.AddAuth(configuration);
            
        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();
        app.UseExceptionHandler(opts => { });
        app.UseHealthChecks("/health");

        app.UseAuth();
            
        return app;
    }
}