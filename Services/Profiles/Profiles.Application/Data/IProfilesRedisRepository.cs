using Profiles.Application.DTO;

namespace Profiles.Application.Data;

public interface IProfilesRedisRepository
{
    public Task CacheProfileAsync(Profile profile);
    public Task CacheBasicInfo(Profile profile);
    public Task<Profile?> GetCachedProfileAsync(Guid profileId);
    public Task<BasicInfoDto?> GetCachedBasicInfoAsync(Guid profileId);
    public Task UpdateProfileAsync(Profile profile);
    public Task UpdateBasicInfoAsync(Profile profile);
    public Task DeleteProfileAsync(Guid profileId);
    public Task DeleteBasicInfoAsync(Guid profileId);
}