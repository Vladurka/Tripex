using System.Text.Json;
using BuildingBlocks.Exceptions;
using Profiles.Application.Data;
using Profiles.Application.DTO;
using StackExchange.Redis;

namespace Profiles.Infrastructure.Data;

public class ProfilesRedisRepository : IProfilesRedisRepository
{
    private readonly IDatabase _redisDb;

    public ProfilesRedisRepository(IConnectionMultiplexer redis) =>
        _redisDb = redis.GetDatabase();
    
    public async Task CacheProfileAsync(Profile profile)
    {
        var cachedProfile = await GetCachedProfileAsync(profile.Id.Value);

        if (cachedProfile == null)
        {
            var key = $"profile:{profile.Id.Value}";
            var value = JsonSerializer.Serialize(profile.ToCachedDto());
            await _redisDb.StringSetAsync(key, value);
        }
    }

    public async Task<Profile?> GetCachedProfileAsync(Guid profileId)
    {
        var key = $"profile:{profileId}";
        var value = await _redisDb.StringGetAsync(key);

        if (value.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<CachedProfileDto>(value!).ToDomain();
    }
    
    public async Task UpdateProfileAsync(Profile profile)
    {
        var cacheKey = $"profile:{profile.Id.Value}";
        
        var serializedProfile = JsonSerializer.Serialize(profile.ToCachedDto());
        
        await _redisDb.StringSetAsync(cacheKey, serializedProfile);
    }

    public async Task DeleteProfileAsync(Guid profileId)
    {
        var cacheKey = $"profile:{profileId}";
        var deleted = await _redisDb.KeyDeleteAsync(cacheKey);

        if (!deleted)
            throw new NotFoundException($"Profile with id {profileId} not found in cache");
    }
}