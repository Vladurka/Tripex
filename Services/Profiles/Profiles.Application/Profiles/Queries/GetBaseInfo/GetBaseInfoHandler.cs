using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Profiles.Application.Profiles.Queries.GetBaseInfo;

public class GetBaseInfoHandler(IProfilesRepository repo) : IQueryHandler<GetBaseInfoQuery, GetBaseInfoResult>
{
    public async Task<GetBaseInfoResult> Handle(GetBaseInfoQuery query, CancellationToken cancellationToken)
    {
        var profile = await repo.GetQueryable()
            .Select(p => new
            {
                p.Id,
                p.ProfileName,
                p.AvatarUrl
            })
            .FirstOrDefaultAsync(p => p.Id ==
                                      ProfileId.Of(query.ProfileId), cancellationToken: cancellationToken);

        if (profile == null)
            return new GetBaseInfoResult(Guid.Empty, "Deleted", Profile.DEFAULT_AVATAR);
        
        return new GetBaseInfoResult(profile.Id.Value, profile.ProfileName.Value, profile.AvatarUrl);
    }
}