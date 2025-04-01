using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Posts.Infrastructure.Data;

namespace Posts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration config)
    {
        var connectionString = config.GetConnectionString("PostgresConnection");

        services.AddDbContext<PostsContext>(options => { options.UseNpgsql(connectionString); });


        return services;
    }
}
