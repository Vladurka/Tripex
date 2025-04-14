namespace Auth.API.Auth.Commands.DeleteUser;

public record DeleteUserCommand(Guid UserId) : ICommand;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator() =>
        RuleFor(x => x.UserId).NotEmpty();
}