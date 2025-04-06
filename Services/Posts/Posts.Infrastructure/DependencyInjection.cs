using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Posts.Application.Data;
using Posts.Infrastructure.Data;

namespace Posts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration config)
    {
        var connectionString = config.GetConnectionString("PostgresConnection");

        services.AddDbContext<PostCountContext>(options => { options.UseNpgsql(connectionString); });

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostCountContext, PostCountContext>();
        
        return services;
    }
}
