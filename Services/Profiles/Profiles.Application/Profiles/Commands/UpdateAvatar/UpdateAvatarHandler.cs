using BuildingBlocks.Exceptions;

namespace Profiles.Application.Profiles.Commands.UpdateAvatar;

public class UpdateAvatarHandler(IProfilesRepository repo, 
    IBlobStorageService blobStorage) : ICommandHandler<UpdateAvatarCommand, UpdateAvatarResult>
{
    public async Task<UpdateAvatarResult> Handle(UpdateAvatarCommand command, CancellationToken cancellationToken)
    {
        var profile = await repo.GetByIdAsync(command.UserId, false) ??
                      throw new NotFoundException("Profile", command.UserId);
    
        if (command.Avatar == null)
        {
            await blobStorage.DeletePhotoAsync(command.UserId, cancellationToken);
            profile.UpdateAvatar(Profile.DEFAULT_AVATAR);
            await repo.SaveChangesAsync();
            return new UpdateAvatarResult(profile.AvatarUrl);
        }
    
        profile.UpdateAvatar(await blobStorage.UploadPhotoAsync(command.Avatar, command.UserId, cancellationToken));
        await repo.SaveChangesAsync(false);

        return new UpdateAvatarResult(profile.AvatarUrl);
    }
}