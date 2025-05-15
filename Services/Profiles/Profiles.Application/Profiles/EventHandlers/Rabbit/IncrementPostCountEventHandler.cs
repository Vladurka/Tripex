using Microsoft.Extensions.Logging;
using Profiles.Application.Profiles.Commands.IncrementPostCount;

namespace Profiles.Application.Profiles.EventHandlers.Rabbit;

public class IncrementPostCountEventHandler(ISender sender,
    ILogger<IncrementPostCountEventHandler> logger) : IConsumer<IncrementPostCountEvent>
{
    public async Task Consume(ConsumeContext<IncrementPostCountEvent> context)
    {
        var message = context.Message;
        await sender.Send(new IncrementPostCountCommand(message.ProfileId), context.CancellationToken);
        logger.LogInformation($"Post count of profile {message.ProfileId} incremented successfully");
    }
}