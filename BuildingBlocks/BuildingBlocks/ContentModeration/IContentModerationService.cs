using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.ContentModeration;

public interface IContentModerationService
{
    public Task<bool> ModeratePhoto(IFormFile sasUrl);
}