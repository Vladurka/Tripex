namespace Profiles.Application.Data;

public interface IProfilesRedisRepository
{
    public Task CacheProfileAsync(Profile profile);
    public Task<Profile?> GetCachedProfileAsync(Guid profileId);
    public Task UpdateProfileAsync(Profile profile);
    public Task DeleteProfileAsync(Guid profileId);
}