using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Data;
using Profiles.Infrastructure.Data;
using StackExchange.Redis;

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
        
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            return ConnectionMultiplexer.Connect(config.GetConnectionString("RedisConnection")!);
        });


        services.AddScoped<IProfilesRepository, ProfilesRepository>();
        services.AddScoped<IProfilesRedisRepository, ProfilesRedisRepository>();

        return services;
    }
}