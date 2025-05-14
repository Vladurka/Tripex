namespace Profiles.Application.Profiles.Commands.CacheBasicInfo;

public class CacheBasicInfoHandler(IProfilesRepository repo,
    IProfilesRedisRepository redisRepo) : ICommandHandler<CacheBasicInfoCommand>
{
    public async Task<Unit> Handle(CacheBasicInfoCommand command, CancellationToken cancellationToken)
    {
        var profile = await repo.GetProfileByIdAsync(ProfileId.Of(command.ProfileId), cancellationToken) ??
            throw new NotFoundException($"Profile with id {command.ProfileId} not found");
        
        await redisRepo.CacheBasicInfo(profile);
        return Unit.Value;
    }
}