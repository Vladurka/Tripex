using Microsoft.EntityFrameworkCore;

namespace Profiles.Application.Profiles.Queries.GetProfiles;

public class GetProfilesHandler(IProfilesRepository repo) 
    : IQueryHandler<GetProfilesQuery, GetProfilesResult> 
{
    public async Task<GetProfilesResult> Handle(GetProfilesQuery query, CancellationToken cancellationToken)
    {
        var profiles = await repo.GetQueryable()
            .Select(p => new GetProfileResult(
                p.Id.Value,
                p.ProfileName.Value,
                p.AvatarUrl,
                p.FirstName,
                p.LastName,
                p.Description,
                p.FollowerCount,
                p.FollowingCount))
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        return new GetProfilesResult(profiles);
    }
}