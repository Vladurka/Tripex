using BuildingBlocks.Exceptions;
using Mapster;
using Profiles.Application.Profiles.Queries;

namespace Profiles.Application.Profiles.Commands.UpdateProfile;

public class UpdateProfileHandler(
    IProfilesRepository repo,
    IPublishEndpoint publishEndpoint) : ICommandHandler<UpdateProfileCommand, GetProfileResult>
{
    public async Task<GetProfileResult> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await repo.GetByIdAsync(command.UserId, false);
        
        if (profile == null)
            throw new NotFoundException("Profile", command.UserId);
        
        if (!string.IsNullOrWhiteSpace(command.ProfileName) && profile.ProfileName.Value != command.ProfileName)
        {
            if (await repo.ProfileNameExistsAsync(command.ProfileName))
                throw new ExistsException(command.ProfileName);

            profile.UpdateUserName(ProfileName.Of(command.ProfileName));

            var message = command.Adapt<UpdateUserNameEvent>();
            await publishEndpoint.Publish(message, cancellationToken);
        }
        
        profile.Update(command.AvatarUrl, command.FirstName, command.LastName, command.Description);

        await repo.UpdateAsync(profile);

        return new GetProfileResult(
            profile.Id.Value,
            profile.ProfileName.Value,
            profile.AvatarUrl,
            profile.FirstName,
            profile.LastName,
            profile.Description
        );
    }
}
