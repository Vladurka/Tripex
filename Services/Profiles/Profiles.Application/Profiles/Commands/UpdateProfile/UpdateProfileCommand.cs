namespace Profiles.Application.Profiles.Commands.UpdateProfile;

public record UpdateProfileCommand(Guid UserId, string UserName, string AvatarUrl,
    string FirstName, string LastName, string Description) : ICommand<UpdateProfileResult>;
public record UpdateProfileResult(bool Succeed);

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.UserName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.UserName)); 

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(500)
            .Matches(@"^(http|https)://.*")
            .When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl));

        RuleFor(x => x.FirstName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
