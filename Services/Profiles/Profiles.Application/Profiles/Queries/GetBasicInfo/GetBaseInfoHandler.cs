using Microsoft.EntityFrameworkCore;

namespace Profiles.Application.Profiles.Queries.GetBasicInfo;

public class GetBaseInfoHandler(IProfilesRepository repo, 
    IProfilesRedisRepository redisRepo) : IQueryHandler<GetBaseInfoQuery, GetBaseInfoResult>
{
    public async Task<GetBaseInfoResult> Handle(GetBaseInfoQuery query, CancellationToken cancellationToken)
    {
        var profileFromCache = await redisRepo.GetCachedBasicInfoAsync(query.ProfileId);

        if (profileFromCache == null)
        {
            var profile = await repo.GetQueryable()
                .Select(p => new
                {
                    p.Id,
                    p.ProfileName,
                    p.AvatarUrl
                })
                .FirstOrDefaultAsync(p => p.Id ==
                    ProfileId.Of(query.ProfileId), cancellationToken: cancellationToken) ??
                throw new NotFoundException("Profile", query.ProfileId);
            
            return new GetBaseInfoResult(query.ProfileId, profile.ProfileName.Value, profile.AvatarUrl);
        }
        
        return new GetBaseInfoResult(query.ProfileId, profileFromCache.ProfileName, profileFromCache.AvatarUrl);
    }
}