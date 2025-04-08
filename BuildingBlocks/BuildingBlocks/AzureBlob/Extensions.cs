using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.AzureBlob;

public static class Extensions
{
    public static IServiceCollection AddBlobStorage(this IServiceCollection services, 
        IConfiguration config)
    {
        services.AddScoped<BlobContainerClient>(sp =>
        {
            var connectionString = config["AzureBlob:ConnectionString"];
            var containerName = config["AzureBlob:ContainerName"];
            
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
                throw new InvalidOperationException("Azure Blob Storage configuration is missing.");
            
            return new BlobContainerClient(connectionString, containerName);
        });

        services.AddScoped<IBlobStorageService, BlobStorageService>();
        return services;
    }
}