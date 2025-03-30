using BuildingBlocks.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Data;
using Profiles.Infrastructure.Data;

namespace Profiles.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration config)
    {
        var connectionString = config.GetConnectionString("PostgresConnection");

        services.AddDbContext<ProfilesContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IProfilesRepository, ProfilesRepository>();

        return services;
    }
}