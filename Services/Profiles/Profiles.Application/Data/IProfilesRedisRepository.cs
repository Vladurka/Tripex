using Profiles.Application.DTO;

namespace Profiles.Application.Data;

public interface IProfilesRedisRepository
{
    public Task CacheProfileAsync(Profile profile);
    public Task CacheBasicInfo(Profile profile);
    public Task<Profile?> GetCachedProfileAsync(ProfileId profileId);
    public Task<BasicInfoDto?> GetCachedBasicInfoAsync(ProfileId profileId);
    public Task UpdateProfileAsync(Profile profile);
    public Task UpdateBasicInfoAsync(Profile profile);
    public Task DeleteProfileAsync(ProfileId profileId);
    public Task DeleteBasicInfoAsync(ProfileId profileId);
}