namespace BuildingBlocks.CORS;

public static class Extensions
{
    public static IServiceCollection AllowFrontend(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); 
            });
        });
        
        return services;
    }
}