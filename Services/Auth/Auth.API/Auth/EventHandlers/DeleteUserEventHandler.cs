namespace Auth.API.Auth.EventHandlers;

public class DeleteUserEventHandler(IUsersRepository repo, ILogger<DeleteUserEventHandler> logger) 
    : IConsumer<DeleteUserEvent>
{
    public async Task Consume(ConsumeContext<DeleteUserEvent> context)
    {
        logger.LogInformation("Deleting user");
        await repo.DeleteAsync(context.Message.ProfileId);
        logger.LogInformation("User deleted");
    }
}