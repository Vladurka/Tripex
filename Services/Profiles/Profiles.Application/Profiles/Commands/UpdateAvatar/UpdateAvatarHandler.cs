using Azure.Storage.Blobs;
using BuildingBlocks.Exceptions;

namespace Profiles.Application.Profiles.Commands.UpdateAvatar;

public class UpdateAvatarHandler(IProfilesRepository repo, 
    BlobContainerClient containerClient) : ICommandHandler<UpdateAvatarCommand, UpdateAvatarResult>
{
    public async Task<UpdateAvatarResult> Handle(UpdateAvatarCommand command, CancellationToken cancellationToken)
    {
        var profile = await repo.GetByIdAsync(command.UserId, false) ??
                      throw new NotFoundException("Profile", command.UserId);

        var blobClient = containerClient.GetBlobClient(command.UserId.ToString());
    
        if (command.Avatar == null)
        {
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            profile.UpdateAvatar(Profile.DEFAULT_AVATAR);
            await repo.SaveChangesAsync();
            return new UpdateAvatarResult(profile.AvatarUrl);
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(command.Avatar.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException("Unsupported file type.");

        await using var stream = command.Avatar.OpenReadStream();
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
        {
            HttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = command.Avatar.ContentType
            }
        }, cancellationToken);
    
        profile.UpdateAvatar(blobClient.Uri.ToString());
        await repo.SaveChangesAsync();

        return new UpdateAvatarResult(profile.AvatarUrl);
    }
}