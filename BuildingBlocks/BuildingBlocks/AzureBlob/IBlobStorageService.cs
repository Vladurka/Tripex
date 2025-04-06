using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.AzureBlob;

public interface IBlobStorageService
{
    public Task<string> UploadPhotoAsync(IFormFile file, Guid id, CancellationToken cancellationToken);
    public Task DeletePhotoAsync(Guid id, CancellationToken cancellationToken);
}