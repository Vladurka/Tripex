using Azure;
using Azure.AI.ContentSafety;

namespace BuildingBlocks.ContentModeration;

public static class Extensions
{
    public static IServiceCollection AddContentModeration(this IServiceCollection services, 
        IConfiguration config)
    {
        services.AddSingleton<ContentSafetyClient>(sp =>
        {
            var endpoint = config["AzureAIContentSafety:Endpoint"];
            var apiKey = config["AzureAIContentSafety:ApiKey"];
            return new ContentSafetyClient(new Uri(endpoint!), new AzureKeyCredential(apiKey!));
        });
        
        services.AddSingleton<IContentModerationService, ContentModerationService>();
        
        return services;
    }
}