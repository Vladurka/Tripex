using Auth.API.Auth.Commands.UpdateUserName;

namespace Auth.API.Auth.EventHandlers;

public class UpdateUserNameEventHandler(ISender sender, ILogger<UpdateUserNameEventHandler> logger) 
    : IConsumer<UpdateUserNameEvent>
{
    public async Task Consume(ConsumeContext<UpdateUserNameEvent> context)
    {
        logger.LogInformation("Updating username");
        var command = new UpdateUserNameCommand
            (context.Message.ProfileId, context.Message.ProfileName);
        await sender.Send(command);
        logger.LogInformation("Username updated " + command.UserName);
    }
}