using Profiles.Application.Profiles.Queries;

namespace Profiles.Application.Profiles.Commands.UpdateProfile;

public record UpdateProfileCommand : ICommand<GetProfileResult>
{
    public Guid ProfileId { get; set; }
    public string? ProfileName { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Description { get; init; }
}

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.ProfileName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.ProfileName));

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
