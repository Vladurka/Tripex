using BuildingBlocks.Messaging.Events.Cache;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Posts.Application.Posts.Commands.CachePosts;

namespace Posts.Application.Posts.EventHandlers.Rabbit;

public class CacheUserEventHandler(ISender sender, ILogger<CacheUserEventHandler> logger) 
    : IConsumer<CacheUserEvent>
{
    public async Task Consume(ConsumeContext<CacheUserEvent> context)
    {
        logger.LogInformation("Caching user");
        var command = new CachePostsCommand(context.Message.ProfileId);
        await sender.Send(command);
        logger.LogInformation("User cached");
    }
}