namespace Profiles.Application.Profiles.Commands.IncrementPostCount;

public class IncrementPostCoutHandler(IProfilesRepository repo, 
    IProfilesRedisRepository redisRepo) : ICommandHandler<IncrementPostCountCommand>
{
    public async Task<Unit> Handle(IncrementPostCountCommand command, CancellationToken cancellationToken)
    {
        var profileId = ProfileId.Of(command.ProfileId);

        var profile = await repo.GetProfileByIdAsync(profileId, cancellationToken, false) ??
            throw new NotFoundException("Profile", profileId.Value);
        
        profile.IncrementPostCount();
        
        List<Task> tasks = new List<Task>
        {
            repo.SaveChangesAsync(cancellationToken),
        };
        
        if(profile.IsCached)
            tasks.Add(redisRepo.UpdateProfileAsync(profile));
        
        await Task.WhenAll(tasks);
        
        return Unit.Value;   
    }
}