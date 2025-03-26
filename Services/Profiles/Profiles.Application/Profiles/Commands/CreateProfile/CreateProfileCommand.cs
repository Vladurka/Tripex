namespace Profiles.Application.Profiles.Commands.CreateProfile;

public record CreateProfileCommand(Guid Id, string ProfileName) : ICommand<CreateProfileResult>;
public record CreateProfileResult(Guid Id);

public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProfileName).NotEmpty();
    }
}