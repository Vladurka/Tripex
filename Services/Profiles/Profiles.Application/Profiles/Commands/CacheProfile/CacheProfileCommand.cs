namespace Profiles.Application.Profiles.Commands.CacheProfile;

public record CacheProfileCommand(Guid ProfileId) : ICommand;

public class CacheProfileCommandValidator : AbstractValidator<CacheProfileCommand>
{
    public CacheProfileCommandValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}