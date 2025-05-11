using Microsoft.EntityFrameworkCore;

namespace Profiles.Application.Profiles.Queries.SearchProfilesByName;

public class SearchProfilesByNameHandler(IProfilesRepository repo) 
    : IQueryHandler<SearchProfilesByNameQuery, SearchProfilesByNameResult>
{
    public async Task<SearchProfilesByNameResult> Handle(SearchProfilesByNameQuery query, CancellationToken cancellationToken)
    {
        var profiles = await repo.GetQueryable()
            .AsNoTracking()
            .Select(p => new GetProfileResult(
                p.Id.Value,
                p.ProfileName.Value, 
                p.AvatarUrl,
                p.FirstName,
                p.LastName,
                p.Description,
                p.FollowersCount,
                p.FollowingCount))
            .ToListAsync(cancellationToken);

        return new SearchProfilesByNameResult(profiles);
    }

}
