using Microsoft.EntityFrameworkCore;

namespace Profiles.Application.Profiles.Queries.GetProfiles;

public class GetProfilesHandler(IProfilesRepository repo) 
    : IQueryHandler<GetProfilesQuery, List<GetProfileResult>> 
{
    public async Task<List<GetProfileResult>> Handle(GetProfilesQuery query, CancellationToken cancellationToken)
    {
        return await repo.GetQueryable()
            .Select(p => new GetProfileResult(
                p.UserName.Value,
                p.AvatarUrl,
                p.FirstName,
                p.LastName,
                p.Description))
            .ToListAsync(cancellationToken);
    }
}