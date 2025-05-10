namespace Profiles.Application.Profiles.Commands.AddFollow;

public record AddFollowCommand(Guid ProfileId, Guid FollowerId) : ICommand;

public class FollowCommandValidator : AbstractValidator<AddFollowCommand>
{
    public FollowCommandValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}