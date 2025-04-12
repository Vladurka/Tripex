namespace Auth.API.Auth.Commands.UpdateUserName;

public class UpdateUserNameHandler(IUsersRepository repo) : ICommandHandler<UpdateUserNameCommand>
{
    public async Task<Unit> Handle(UpdateUserNameCommand command, CancellationToken cancellationToken)
    {
        var user = await repo.GetUserByIdAsync(command.UserId);

        if (user == null)
            throw new NotFoundException("User", command.UserId);

        user.UserName = command.UserName;
        await repo.SaveChangesAsync();
        return Unit.Value;
    }
}