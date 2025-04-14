using Auth.API.Auth.Commands.DeleteUser;

namespace Auth.API.Auth.EventHandlers;

public class DeleteUserEventHandler(ISender sender, ILogger<DeleteUserEventHandler> logger) 
    : IConsumer<DeleteUserEvent>
{
    public async Task Consume(ConsumeContext<DeleteUserEvent> context)
    {
        logger.LogInformation($"Deleting user with id {context.Message.ProfileId}");
        var command = new DeleteUserCommand(context.Message.ProfileId);
        await sender.Send(command);
        logger.LogInformation("User deleted");
    }
}