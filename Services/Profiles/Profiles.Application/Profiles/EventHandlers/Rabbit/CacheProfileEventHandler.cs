using BuildingBlocks.Messaging.Events.Cache;
using Microsoft.Extensions.Logging;
using Profiles.Application.Profiles.Commands.CacheProfile;

namespace Profiles.Application.Profiles.EventHandlers.Rabbit;

public class CacheProfileEventHandler(ISender sender, ILogger<CacheProfileEventHandler> logger)
    : IConsumer<CacheUserEvent>
{
    public async Task Consume(ConsumeContext<CacheUserEvent> context)
    {
        logger.LogInformation("Caching profile");
        var command = new CacheProfileCommand(context.Message.ProfileId);
        await sender.Send(command, context.CancellationToken);
        logger.LogInformation("Profile cached");
    }
}