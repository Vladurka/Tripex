using BuildingBlocks.Exceptions;
using Cassandra.Data.Linq;
using Posts.Application.Data;
using Posts.Application.Posts.DTO;
using Posts.Application.Posts.Extensions;
using Posts.Domain.Models;
using Posts.Domain.ValueObjects;

namespace Posts.Infrastructure.Data;

public class PostRepository : IPostRepository
{
    private readonly Table<PostDb> _posts;
    private readonly Table<PostCountDb> _postCount;

    public PostRepository()
    {
        var session = CassandraSession.Connect();
        _posts = new Table<PostDb>(session);
        _postCount = new Table<PostCountDb>(session);
    }
    
    public async Task AddPostAsync(PostDb post) =>
        await _posts.Insert(post).ExecuteAsync();

    public async Task<Post?> GetPostByIdAsync(PostId id)
    {
        var result = await _posts
            .Where(p => p.Id == id.Value)
            .ExecuteAsync();

        var db = result.FirstOrDefault();
        return db?.ToDomain();
    }

    public async Task<IEnumerable<Post>> GetPostsByUserAsync(ProfileId profileId)
    { 
        var result = await _posts
            .Where(p => p.ProfileId == profileId.Value)
            .ExecuteAsync();

        return result.Select(PostMapper.ToDomain);
    }
    
    public async Task<IEnumerable<Guid>> GetPostIdsByUserAsync(ProfileId profileId)
    { 
        var result = await _posts
            .Where(p => p.ProfileId == profileId.Value)
            .Select(p => p.Id)
            .ExecuteAsync();

        return result;
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        var result = await _posts.Select(_ => _).ExecuteAsync();
        return result.Select(PostMapper.ToDomain).ToList();
    }

    public async Task DeletePostAsync(PostId id)
    {
        await _posts
            .Where(p => p.Id == id.Value)
            .Delete()
            .ExecuteAsync();
    }
    
    public async Task DeletePostsAsync(ProfileId profileId)
    {
        await _posts
            .Where(p => p.ProfileId == profileId.Value)
            .Delete()
            .ExecuteAsync();
    }
    
    public async Task IncrementPostCount(ProfileId profileId)
    {
        var post = await _postCount.FirstOrDefault(p => p.ProfileId == profileId.Value).ExecuteAsync();

        if (post == null)
        {
            post = new PostCountDb
            {
                ProfileId = profileId.Value,
                Count = 1
            };
            await _postCount.Insert(post).ExecuteAsync();
        }
        else
        {
            post.Count += 1;
            
            await _postCount
                .Where(p => p.ProfileId == profileId.Value)
                .Select(p => new PostCountDb { Count = post.Count })
                .Update()
                .ExecuteAsync();
        }
    }
    
    public async Task DecrementPostCount(ProfileId profileId)
    {
        var postCount = await _postCount
            .FirstOrDefault(p => p.ProfileId == profileId.Value).ExecuteAsync();

        if (postCount == null)
            throw new NotFoundException("PostCount", profileId);
        
        postCount.Count -= 1;
        
        await _postCount
            .Where(p => p.ProfileId == profileId.Value)
            .Select(p => new PostCountDb { Count = postCount.Count })
            .Update()
            .ExecuteAsync();
    }

    public async Task DeletePostCountAsync(ProfileId profileId)
    {
        await _postCount
            .Where(p => p.ProfileId == profileId.Value)
            .Delete()
            .ExecuteAsync();
    }

    public async Task<int> GetPostCount(ProfileId profileId)
    {
        var profile = await _postCount
            .FirstOrDefault(p => p.ProfileId == profileId.Value).ExecuteAsync();
        
        if(profile == null)
            return 0;
            
        return profile.Count;
    }
}