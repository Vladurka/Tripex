using BuildingBlocks.Messaging.Events.Cache;
using Profiles.Application.Profiles.Commands.CacheProfile;

namespace Profiles.Application.Profiles.EventHandlers.Rabbit;

public class CacheProfileEventHandler(ISender sender) : IConsumer<CacheUserEvent>
{
    public async Task Consume(ConsumeContext<CacheUserEvent> context)
    {
        var command = new CacheProfileCommand(context.Message.ProfileId);
        await sender.Send(command);
    }
}