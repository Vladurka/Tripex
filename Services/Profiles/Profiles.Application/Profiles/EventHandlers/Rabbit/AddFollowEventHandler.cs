using Microsoft.Extensions.Logging;
using Profiles.Application.Profiles.Commands.AddFollow;

namespace Profiles.Application.Profiles.EventHandlers.Rabbit;

public class AddFollowEventHandler(ISender sender, 
    ILogger<AddFollowEventHandler> logger) : IConsumer<AddFollowEvent>
{
    public async Task Consume(ConsumeContext<AddFollowEvent> context)
    {
        var message = context.Message;
        await sender.Send(new AddFollowCommand(message.ProfileId, message.FollowerId), 
            context.CancellationToken);
        logger.LogInformation("Follow added successfully");
    }
}