namespace Profiles.Application.Profiles.Commands.CacheProfile;

public class CacheProfileHandler(IProfilesRedisRepository redisRepo, IProfilesRepository repo) 
    : ICommandHandler<CacheProfileCommand>
{
    public async Task<Unit> Handle(CacheProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await repo.GetProfileByIdAsync(command.ProfileId) ??
                      throw new NotFoundException("Profile", command.ProfileId);

        await redisRepo.CacheProfileAsync(profile);

        profile.SetIsCached(true);
        await repo.SaveChangesAsync();
        
        return Unit.Value;
    }
}