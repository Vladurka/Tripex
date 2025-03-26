using BuildingBlocks.Exceptions;

namespace Profiles.Application.Profiles.Queries.GetProfileById;

public class GetProfileByIdHandler(IProfilesRepository repo) 
    : IQueryHandler<GetProfileByIdQuery, GetProfileResult>
{
    public async Task<GetProfileResult> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken)
    {
        var profile = await repo.GetByIdAsync(query.UserId, true)
                      ?? throw new NotFoundException("Profile", query.UserId);

        return new GetProfileResult(
            profile.Id.Value,
            profile.ProfileName.Value,
            profile.AvatarUrl,
            profile.FirstName,
            profile.LastName,
            profile.Description
        );
    }
}