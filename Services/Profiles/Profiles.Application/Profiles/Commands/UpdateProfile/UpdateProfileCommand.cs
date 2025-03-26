using Profiles.Application.Profiles.Queries;

namespace Profiles.Application.Profiles.Commands.UpdateProfile;

public record UpdateProfileCommand(
    Guid UserId, 
    string? ProfileName,
    string? AvatarUrl,
    string? FirstName,
    string? LastName, 
    string? Description) : ICommand<GetProfileResult>;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.ProfileName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.ProfileName));

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(500)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
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
