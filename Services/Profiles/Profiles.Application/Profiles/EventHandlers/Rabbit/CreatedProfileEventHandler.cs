using Microsoft.Extensions.Logging;
using Profiles.Application.Profiles.Commands.CreateProfile;

namespace Profiles.Application.Profiles.EventHandlers.Rabbit;

public class CreatedProfileEventHandler(ISender sender): IConsumer<CreateProfileEvent>
{
    public async Task Consume(ConsumeContext<CreateProfileEvent> context)
    {
        var command = new CreateProfileCommand(context.Message.UserId, 
            context.Message.UserName);
        
        await sender.Send(command);
    }
}