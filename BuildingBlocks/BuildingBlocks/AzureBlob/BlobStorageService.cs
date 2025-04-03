using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.AzureBlob;

public class BlobStorageService(BlobContainerClient containerClient) : IBlobStorageService
{
    public async Task<string> UploadPhotoAsync(IFormFile file, Guid id, CancellationToken cancellationToken )
    {
        var blobClient = containerClient.GetBlobClient(id.ToString());
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException("Unsupported file type.");

        await using var stream = file.OpenReadStream();
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
        {
            HttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = file.ContentType
            }
        }, cancellationToken);
        
        return blobClient.Uri.ToString();
    }

    public async Task DeletePhotoAsync(Guid id, CancellationToken cancellationToken )
    {
        var blobClient = containerClient.GetBlobClient(id.ToString());
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}