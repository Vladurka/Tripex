namespace Profiles.Application.Profiles.Commands.IncrementPostCount;

public record IncrementPostCountCommand(Guid ProfileId) : ICommand;

public class IncrementPostCountCommandValidator : AbstractValidator<IncrementPostCountCommand>
{
    public IncrementPostCountCommandValidator() =>
        RuleFor(command => command.ProfileId);
}