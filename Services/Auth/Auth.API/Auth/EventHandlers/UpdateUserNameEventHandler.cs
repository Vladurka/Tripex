namespace Auth.API.Auth.EventHandlers;

public class UpdateUserNameEventHandler(IUsersRepository repo, ILogger<UpdateUserNameEventHandler> logger) 
    : IConsumer<UpdateUserNameEvent>
{
    public async Task Consume(ConsumeContext<UpdateUserNameEvent> context)
    {
        logger.LogInformation("Updating username");
        var user = await repo.GetUserByIdAsync(context.Message.UserId);

        if (user == null)
            throw new NotFoundException("User", context.Message.UserId);

        user.UserName = context.Message.ProfileName;
        await repo.SaveChangesAsync();
        logger.LogInformation("Username updated");
    }
}