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
        
        var fileName = command.Avatar.FileName;

        var blobClient = containerClient.GetBlobClient(fileName);

        await using var stream = command.Avatar.OpenReadStream();
        await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
        {
            HttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = command.Avatar.ContentType
            }
        }, cancellationToken);

        profile.UpdateAvatar(blobClient.Uri.ToString());
        await repo.SaveChangesAsync();

        return new UpdateAvatarResult(true);
    }
}