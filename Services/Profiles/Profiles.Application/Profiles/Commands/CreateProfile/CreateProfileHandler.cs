namespace Profiles.Application.Profiles.Commands.CreateProfile;

public class CreateProfileHandler(IProfilesRepository repo) : ICommandHandler<CreateProfileCommand, CreateProfileResult>
{
    public async Task<CreateProfileResult> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = Profile.Create(ProfileId.Of(command.Id), ProfileName.Of(command.ProfileName), null, null,
            null, null);
        await repo.AddAsync(profile);

        return new CreateProfileResult(command.Id);
    }
}