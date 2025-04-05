using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;

namespace BuildingBlocks.Auth;

public static class Extensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IJwtHelper, JwtHelper>();
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["JwtOptions:Issuer"],
                    ValidAudience = config["JwtOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtOptions:SecurityKey"]!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[config["JwtOptions:TokenName"]!];
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
        
        return services;
    }
    
    public static WebApplication UseAuth(this WebApplication app)
    {
        app.UseAuthentication();

        app.UseAuthorization();
            
        return app;
    }
}