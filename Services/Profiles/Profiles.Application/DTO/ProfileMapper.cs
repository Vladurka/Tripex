namespace Profiles.Application.DTO;

public static class ProfileMapper
{
    public static CachedProfileDto ToCachedDto(this Profile profile)
    {
        return new CachedProfileDto
        {
            Id = profile.Id.Value,
            ProfileName = profile.ProfileName.Value,
            AvatarUrl = profile.AvatarUrl,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Description = profile.Description
        };
    }

    public static Profile ToDomain(this CachedProfileDto? dto)
    {
        return Profile.Create(
           ProfileId.Of(dto.Id),
           ProfileName.Of(dto.ProfileName),
           dto.AvatarUrl,
           dto.FirstName,
           dto.LastName,
           dto.Description
        );
    }
}