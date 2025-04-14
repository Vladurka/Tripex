namespace Auth.API.Auth.Commands.DeleteUser;

public class DeleteUserHandler(IUsersRepository repo) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        await repo.DeleteAsync(command.UserId, cancellationToken);
        return Unit.Value;
    }
}