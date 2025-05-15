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
            Description = profile.Description,
            FollowerCount = profile.FollowerCount,
            FollowingCount = profile.FollowingCount,
            PostCount = profile.PostCount
        };
    }
    
    public static BasicInfoDto ToBasicInfoDto(this Profile profile)
    {
        return new BasicInfoDto
        {
            ProfileName = profile.ProfileName.Value,
            AvatarUrl = profile.AvatarUrl,
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
           dto.Description,
           dto.FollowerCount,
           dto.FollowingCount,
           dto.PostCount
        );
    }
}