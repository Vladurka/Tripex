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
        
        if (!string.IsNullOrWhiteSpace(command.UserName) && profile.UserName.Value != command.UserName)
        {
            if (await repo.UsernameExistsAsync(command.UserName))
                throw new ExistsException(command.UserName);

            profile.UpdateUserName(UserName.Of(command.UserName));

            var message = command.Adapt<UpdateUserNameEvent>();
            await publishEndpoint.Publish(message, cancellationToken);
        }
        
        profile.Update(command.AvatarUrl, command.FirstName, command.LastName, command.Description);

        await repo.UpdateAsync(profile);

        return new UpdateProfileResult(true);
    }
}
