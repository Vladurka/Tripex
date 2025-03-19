using Profiles.Application.Data;
using Profiles.Domain.ValueObjects;

namespace Profiles.Application.Profiles.Commands.CreateProfile;

public class CreateProfileHandler(IProfilesRepository repo) : ICommandHandler<CreateProfileCommand, CreateProfileResult>
{
    public async Task<CreateProfileResult> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = Profile.Create(ProfileId.Of(command.Id), UserName.Of(command.UserName));
        await repo.AddAsync(profile);

        return new CreateProfileResult(command.Id);
    }
}