using Microsoft.EntityFrameworkCore;

namespace Profiles.Application.Profiles.Queries.GetProfiles;

public class GetProfilesHandler(IProfilesRepository repo) 
    : IQueryHandler<GetProfilesQuery, GetProfilesResult> 
{
    public async Task<GetProfilesResult> Handle(GetProfilesQuery query, CancellationToken cancellationToken)
    {
        var profiles = await repo.GetQueryable()
            .Select(p => new GetProfileResult(
                p.UserName.Value,
                p.AvatarUrl,
                p.FirstName,
                p.LastName,
                p.Description))
            .ToListAsync(cancellationToken);

        return new GetProfilesResult(profiles);
    }
}