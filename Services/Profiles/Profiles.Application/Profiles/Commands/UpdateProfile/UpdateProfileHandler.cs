using BuildingBlocks.Exceptions;
using Mapster;

namespace Profiles.Application.Profiles.Commands.UpdateProfile;

public class UpdateProfileHandler(
    IProfilesRepository repo,
    IPublishEndpoint publishEndpoint) : ICommandHandler<UpdateProfileCommand, UpdateProfileResult>
{
    public async Task<UpdateProfileResult> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await repo.GetByIdAsync(command.UserId);

        if (profile == null)
            throw new NotFoundException("Profile", command.UserId);

        if (!string.IsNullOrWhiteSpace(command.UserName) && 
            profile.UserName.Value != command.UserName)
        {
            var message = command.Adapt<UpdateUserNameEvent>();
            await publishEndpoint.Publish(message, cancellationToken);
        }

        var newProfile = Profile.Create(
            profile.Id, 
            string.IsNullOrWhiteSpace(command.UserName) ? profile.UserName : UserName.Of(command.UserName),
            string.IsNullOrWhiteSpace(command.AvatarUrl) ? profile.AvatarUrl : command.AvatarUrl,
            string.IsNullOrWhiteSpace(command.FirstName) ? profile.FirstName : command.FirstName,
            string.IsNullOrWhiteSpace(command.LastName) ? profile.LastName : command.LastName,
            string.IsNullOrWhiteSpace(command.Description) ? profile.Description : command.Description
        );

        await repo.UpdateAsync(newProfile);

        return new UpdateProfileResult(true);
    }
}