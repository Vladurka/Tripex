using Microsoft.EntityFrameworkCore;

namespace Profiles.Application.Profiles.Queries.GetBasicInfo;

public class GetBaseInfoHandler(IProfilesRepository repo, 
    IProfilesRedisRepository redisRepo) : IQueryHandler<GetBaseInfoQuery, GetBaseInfoResult>
{
    public async Task<GetBaseInfoResult> Handle(GetBaseInfoQuery query, CancellationToken cancellationToken)
    {
        var profileId = ProfileId.Of(query.ProfileId);       
        
        var profileFromCache = 
            await redisRepo.GetCachedBasicInfoAsync(profileId);

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
                    profileId, cancellationToken: cancellationToken) ??
                throw new NotFoundException("Profile", query.ProfileId);
            
            return new GetBaseInfoResult(query.ProfileId, profile.ProfileName.Value, profile.AvatarUrl);
        }
        
        return new GetBaseInfoResult(query.ProfileId, profileFromCache.ProfileName, profileFromCache.AvatarUrl);
    }
}