using BuildingBlocks.Exceptions;

namespace Profiles.Application.Profiles.Commands.CacheProfile;

public class CacheProfileHandler(IProfilesRedisRepository redisRepo, IProfilesRepository repo) 
    : ICommandHandler<CacheProfileCommand, CacheProfileResult>
{
    public async Task<CacheProfileResult> Handle(CacheProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await repo.GetByIdAsync(command.ProfileId) ??
                      throw new NotFoundException("Profile", command.ProfileId);

        await redisRepo.CacheProfileAsync(profile);
        
        return new CacheProfileResult(true);
    }
}