namespace Profiles.Application.Profiles.Commands.CacheBasicInfo;

public record CacheBasicInfoCommand(Guid ProfileId) : ICommand;

public class CacheBasicInfoCommandValidator : AbstractValidator<CacheBasicInfoCommand>
{
    public CacheBasicInfoCommandValidator()=>
        RuleFor(x => x.ProfileId).NotEmpty();
}