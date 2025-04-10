using BuildingBlocks.Messaging.Events.Cache;
using MassTransit;
using MediatR;
using Posts.Application.Posts.Commands.CachePosts;

namespace Posts.Application.Posts.EventHandlers.Rabbit;

public class CacheUserEventHandler(ISender sender) : IConsumer<CacheUserEvent>
{
    public async Task Consume(ConsumeContext<CacheUserEvent> context)
    {
        var command = new CachePostsCommand(context.Message.ProfileId);
        await sender.Send(command);
    }
}