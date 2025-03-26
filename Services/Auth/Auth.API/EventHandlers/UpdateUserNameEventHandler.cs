namespace Auth.API.EventHandlers;

public class UpdateUserNameEventHandler(IUsersRepository repo) : IConsumer<UpdateUserNameEvent>
{
    public async Task Consume(ConsumeContext<UpdateUserNameEvent> context)
    {
        var user = await repo.GetUserByIdAsync(context.Message.UserId);

        if (user == null)
            throw new NotFoundException("User", context.Message.UserId);

        user.UserName = context.Message.ProfileName;
        await repo.SaveChangesAsync();
    }
}