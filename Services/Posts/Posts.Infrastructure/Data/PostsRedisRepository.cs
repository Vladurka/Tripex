using System.Text.Json;
using BuildingBlocks.Exceptions;
using Posts.Application.Data;
using Posts.Application.Posts.DTO;
using Posts.Application.Posts.Extensions;
using Posts.Domain.Models;
using Posts.Domain.ValueObjects;
using StackExchange.Redis;

namespace Posts.Infrastructure.Data;

public class PostsRedisRepository : IPostsRedisRepository
{
    private readonly IDatabase _redisDb;

    public PostsRedisRepository(IConnectionMultiplexer redis) =>
        _redisDb = redis.GetDatabase();

    public async Task CachePostsAsync(IEnumerable<Post> posts, ProfileId profileId)
    {
        var key = ConvertToKey(profileId);
        var value = JsonSerializer.Serialize(posts.Select(p => p.ToCachedPostDto()));
        await _redisDb.StringSetAsync(key, value);
    }

    public async Task<IEnumerable<Post>> GetCachedPostsAsync(ProfileId profileId)
    {
        var key = ConvertToKey(profileId);
        var value = await _redisDb.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return [];

        return JsonSerializer.Deserialize<IEnumerable<CachedPostDto>>(value!)!
            .Select(p => p.ToDomain());
    }

    public async Task<bool> ArePostsCachedAsync(ProfileId profileId) =>
        await _redisDb.KeyExistsAsync(ConvertToKey(profileId));

    public async Task AddPostAsync(Post post)
    {
        var key = ConvertToKey(post.ProfileId);
        var posts = await GetCachedPostsAsync(post.ProfileId);

        var updatedPosts = posts.Append(post).Select(p => p.ToCachedPostDto());
        var serialized = JsonSerializer.Serialize(updatedPosts);

        await _redisDb.StringSetAsync(key, serialized);
    }

    public async Task DeletePostAsync(PostId postId, ProfileId profileId)
    {
        if (await ArePostsCachedAsync(profileId))
        {
            var key = ConvertToKey(profileId);
            var posts = await GetCachedPostsAsync(profileId);

            var updatedPosts = posts
                .Where(p => p.Id.Value != postId.Value)
                .Select(p => p.ToCachedPostDto());

            var serialized = JsonSerializer.Serialize(updatedPosts);
            await _redisDb.StringSetAsync(key, serialized);
        }
    }
    
    public async Task DeletePostsAsync(ProfileId profileId)
    {
        if (await ArePostsCachedAsync(profileId))
        {
            var key = ConvertToKey(profileId);
            await _redisDb.KeyDeleteAsync(key);
        }
    }
    
    private string ConvertToKey(ProfileId profileId) =>
        $"profile:{profileId.Value}";
}
