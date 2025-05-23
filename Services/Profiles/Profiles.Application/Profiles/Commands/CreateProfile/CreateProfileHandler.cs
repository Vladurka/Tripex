namespace Profiles.Application.Profiles.Commands.CreateProfile;

public class CreateProfileHandler(IProfilesRepository repo) 
    : ICommandHandler<CreateProfileCommand>
{
    public async Task<Unit> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = Profile.Create(ProfileId.Of(command.Id), ProfileName.Of(command.ProfileName));
        await repo.CreateProfileAsync(profile, cancellationToken);

        return Unit.Value;
    }
}