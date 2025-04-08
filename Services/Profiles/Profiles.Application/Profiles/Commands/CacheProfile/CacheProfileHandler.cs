namespace Profiles.Application.Profiles.Commands.CacheProfile;

public class CacheProfileHandler(IProfilesRedisRepository redisRepo, IProfilesRepository repo) 
    : ICommandHandler<CacheProfileCommand, CacheProfileResult>
{
    public async Task<CacheProfileResult> Handle(CacheProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await repo.GetProfileByIdAsync(command.ProfileId) ??
                      throw new NotFoundException("Profile", command.ProfileId);

        await redisRepo.CacheProfileAsync(profile);

        profile.SetIsCached(true);
        await repo.SaveChangesAsync();
        
        return new CacheProfileResult(true);
    }
}