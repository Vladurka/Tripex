namespace Profiles.Application.Profiles.Queries.GetProfiles;

public record GetProfilesQuery() : IQuery<GetProfilesResult>; 
public record GetProfilesResult(IEnumerable<GetProfileResult> Profiles);