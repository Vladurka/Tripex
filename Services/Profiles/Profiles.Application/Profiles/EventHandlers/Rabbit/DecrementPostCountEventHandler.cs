using Microsoft.Extensions.Logging;
using Profiles.Application.Profiles.Commands.DecrementPostCount;

namespace Profiles.Application.Profiles.EventHandlers.Rabbit;

public class DecrementPostCountEventHandler(ISender sender,
    ILogger<DecrementPostCountEventHandler> logger) : IConsumer<DecrementPostCountEvent>
{
    public async Task Consume(ConsumeContext<DecrementPostCountEvent> context)
    {
        var message = context.Message;
        await sender.Send(new DecrementPostCountCommand(message.ProfileId), context.CancellationToken);
        logger.LogInformation("Post count decremented successfully");
    }
}
