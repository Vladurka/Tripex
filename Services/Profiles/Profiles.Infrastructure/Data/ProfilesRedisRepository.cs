using System.Text.Json;
using BuildingBlocks.Exceptions;
using Profiles.Application.Data;
using Profiles.Application.DTO;
using Profiles.Domain.ValueObjects;
using StackExchange.Redis;

namespace Profiles.Infrastructure.Data;

public class ProfilesRedisRepository : IProfilesRedisRepository
{
    private readonly IDatabase _redisDb;

    public ProfilesRedisRepository(IConnectionMultiplexer redis) =>
        _redisDb = redis.GetDatabase();

    public async Task CacheProfileAsync(Profile profile)
    {
        var key = GetProfileKey(profile.Id);
        var value = JsonSerializer.Serialize(profile.ToCachedDto());
        await _redisDb.StringSetAsync(key, value, when: When.NotExists);
    }

    public async Task CacheBasicInfo(Profile profile)
    {
        var key = GetBasicInfoKey(profile.Id);
        var value = JsonSerializer.Serialize(profile.ToBasicInfoDto());
        await _redisDb.StringSetAsync(key, value, when: When.NotExists);
    }

    public async Task<Profile?> GetCachedProfileAsync(ProfileId profileId)
    {
        var value = await _redisDb.StringGetAsync(GetProfileKey(profileId));
        return value.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CachedProfileDto>(value!)?.ToDomain();
    }

    public async Task<BasicInfoDto?> GetCachedBasicInfoAsync(ProfileId profileId)
    {
        var value = await _redisDb.StringGetAsync(GetBasicInfoKey(profileId));
        return value.IsNullOrEmpty ? null : JsonSerializer.Deserialize<BasicInfoDto>(value!);
    }

    public async Task UpdateProfileAsync(Profile profile)
    {
        var key = GetProfileKey(profile.Id);
        var value = JsonSerializer.Serialize(profile.ToCachedDto());
        await _redisDb.StringSetAsync(key, value);
    }

    public async Task UpdateBasicInfoAsync(Profile profile)
    {
        var key = GetBasicInfoKey(profile.Id);
        if (await _redisDb.KeyExistsAsync(key))
        {
            var value = JsonSerializer.Serialize(profile.ToBasicInfoDto());
            await _redisDb.StringSetAsync(key, value);
        }
    }

    public async Task DeleteProfileAsync(ProfileId profileId)
    {
        var key = GetProfileKey(profileId);
        var deleted = await _redisDb.KeyDeleteAsync(key);
        
        if (!deleted)
            throw new NotFoundException($"Profile with id {profileId.Value} not found in cache");
    }

    public async Task DeleteBasicInfoAsync(ProfileId profileId)
    {
        var key = GetBasicInfoKey(profileId);
        await _redisDb.KeyDeleteAsync(key);
    }

    private static string GetProfileKey(ProfileId profileId) => $"profile:{profileId.Value}";
    private static string GetBasicInfoKey(ProfileId profileId) => $"basicInfo:{profileId.Value}";
}
