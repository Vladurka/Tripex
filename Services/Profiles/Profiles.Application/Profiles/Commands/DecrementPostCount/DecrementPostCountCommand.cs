namespace Profiles.Application.Profiles.Commands.DecrementPostCount;

public record DecrementPostCountCommand(Guid ProfileId) : ICommand;

public class DecrementPostCountCommandValidator : AbstractValidator<DecrementPostCountCommand>
{
    public DecrementPostCountCommandValidator() =>
        RuleFor(command => command.ProfileId).NotEmpty();
}
