namespace Auth.API.EventHandlers;

public class DeleteUserEventHandler(IUsersRepository repo) : IConsumer<DeleteUserEvent>
{
    public async Task Consume(ConsumeContext<DeleteUserEvent> context) =>
        await repo.DeleteAsync(context.Message.UserId);
}