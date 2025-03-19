using Profiles.Application.Profiles.Commands.CreateProfile;

namespace Profiles.Application.Profiles.EventHandlers.Rabbit;

public class UserCreatedEventHandler(ISender sender) : IConsumer<CreateProfileEvent>
{
    public async Task Consume(ConsumeContext<CreateProfileEvent> context)
    {
        var command = new CreateProfileCommand(context.Message.UserId, 
            context.Message.UserName);
        
        await sender.Send(command);
    }
}