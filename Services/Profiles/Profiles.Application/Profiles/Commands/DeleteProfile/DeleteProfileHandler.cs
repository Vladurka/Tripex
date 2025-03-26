using Mapster;
using Microsoft.Extensions.Logging;

namespace Profiles.Application.Profiles.Commands.DeleteProfile;

public class DeleteProfileHandler(IProfilesRepository repo, 
    IPublishEndpoint publishEndpoint, ILogger<DeleteProfileHandler> logger) : ICommandHandler<DeleteProfileCommand, DeleteProfileResult>
{
    public async Task<DeleteProfileResult> Handle(DeleteProfileCommand command, CancellationToken cancellationToken)
    {
        var eventMessage = command.Adapt<DeleteUserEvent>();
        
        await publishEndpoint.Publish(eventMessage, cancellationToken);
        logger.LogInformation("DeleteUserEvent was sent with id " + eventMessage.UserId);
        
        await repo.RemoveAsync(command.UserId);

        return new DeleteProfileResult(true);
    }
}