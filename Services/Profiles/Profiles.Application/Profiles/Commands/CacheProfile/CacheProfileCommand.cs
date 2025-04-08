namespace Profiles.Application.Profiles.Commands.CacheProfile;

public record CacheProfileCommand(Guid ProfileId) : ICommand<CacheProfileResult>;
public record CacheProfileResult(bool IsSucceed);

public class CacheProfileCommandValidator : AbstractValidator<CacheProfileCommand>
{
    public CacheProfileCommandValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}