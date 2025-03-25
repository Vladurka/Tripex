using BuildingBlocks.Exceptions;

namespace Profiles.Application.Profiles.Queries.GetProfileByName;

public class GetProfileByUserNameHandler(IProfilesRepository repo) 
    : IQueryHandler<GetProfileByUserNameQuery, GetProfileResult>
{
    public async Task<GetProfileResult> Handle(GetProfileByUserNameQuery query, CancellationToken cancellationToken)
    {
        var profile = await repo.GetByUserNameAsync(query.UserName) 
                      ?? throw new NotFoundException("Profile", query.UserName);

        return new GetProfileResult(
            profile.UserName.Value,
            profile.AvatarUrl,
            profile.FirstName,
            profile.LastName,
            profile.Description
        );
    }
}
