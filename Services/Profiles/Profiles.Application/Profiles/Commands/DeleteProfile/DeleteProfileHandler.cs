using Mapster;

namespace Profiles.Application.Profiles.Commands.DeleteProfile;

public class DeleteProfileHandler(IProfilesRepository repo, 
    IPublishEndpoint publishEndpoint) : ICommandHandler<DeleteProfileCommand, DeleteProfileResult>
{
    public async Task<DeleteProfileResult> Handle(DeleteProfileCommand command, CancellationToken cancellationToken)
    {
        await repo.RemoveAsync(command.UserId);
        var eventMessage = command.Adapt<DeleteUserEvent>();

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        return new DeleteProfileResult(true);
    }
}