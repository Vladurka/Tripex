using BuildingBlocks.Messaging.Events.Profiles;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Posts.Application.Posts.Commands.DeletePostsByProfile;

namespace Posts.Application.Posts.EventHandlers.Rabbit;

public class DeleteUserPostsEventHandler(ISender sender, ILogger<DeleteUserPostsEventHandler> logger)
    : IConsumer<DeleteUserEvent>
{
    public async Task Consume(ConsumeContext<DeleteUserEvent> context)
    {
        logger.LogInformation("Deleting posts");
        var command = new DeletePostsByProfileCommand(context.Message.ProfileId);
        await sender.Send(command);
        logger.LogInformation("Posts deleted");
    }
}