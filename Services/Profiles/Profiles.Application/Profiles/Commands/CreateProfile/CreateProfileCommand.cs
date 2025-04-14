namespace Profiles.Application.Profiles.Commands.CreateProfile;

public record CreateProfileCommand(Guid Id, string ProfileName) : ICommand;

public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProfileName).NotEmpty();
    }
}