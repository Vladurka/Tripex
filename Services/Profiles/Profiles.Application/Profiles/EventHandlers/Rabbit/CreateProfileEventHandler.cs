using Microsoft.Extensions.Logging;
using Profiles.Application.Profiles.Commands.CreateProfile;

namespace Profiles.Application.Profiles.EventHandlers.Rabbit;

public class CreateProfileEventHandler(ISender sender, ILogger<CreateProfileEventHandler> logger) 
    : IConsumer<CreateProfileEvent>
{
    public async Task Consume(ConsumeContext<CreateProfileEvent> context)
    {
        logger.LogInformation("Creating profile");
        
        var command = new CreateProfileCommand(context.Message.UserId, 
            context.Message.UserName);
        
        await sender.Send(command);
        
        logger.LogInformation("Profile created");
    }
}