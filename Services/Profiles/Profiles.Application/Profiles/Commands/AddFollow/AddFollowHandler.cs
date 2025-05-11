namespace Profiles.Application.Profiles.Commands.AddFollow;

public class AddFollowHandler(IProfilesRepository repo) : ICommandHandler<AddFollowCommand>
{
    public async Task<Unit> Handle(AddFollowCommand command, CancellationToken cancellationToken)
    {
        var profile = await repo.GetProfileByIdAsync(command.ProfileId, cancellationToken, false) ??
                      throw new NotFoundException("Profile", command.ProfileId);
        
        var follower = await repo.GetProfileByIdAsync(command.FollowerId, cancellationToken, false) ??
                      throw new NotFoundException("Profile", command.FollowerId);
        
        profile.AddFollower();
        follower.AddFollowing();
        await repo.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}