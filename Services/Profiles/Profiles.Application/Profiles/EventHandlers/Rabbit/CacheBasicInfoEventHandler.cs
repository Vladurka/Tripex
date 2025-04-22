using BuildingBlocks.Messaging.Events.Cache;
using Microsoft.Extensions.Logging;
using Profiles.Application.Profiles.Commands.CacheBasicInfo;

namespace Profiles.Application.Profiles.EventHandlers.Rabbit;

public class CacheBasicInfoEventHandler(ISender sender, 
    ILogger<CacheBasicInfoEventHandler> logger) : IConsumer<CacheBasicInfoEvent>
{
    public async Task Consume(ConsumeContext<CacheBasicInfoEvent> context)
    {
        logger.LogInformation("Caching basic info");
        var command = new CacheBasicInfoCommand(context.Message.ProfileId);
        await sender.Send(command);
        logger.LogInformation("Basic info cached");
    }
}