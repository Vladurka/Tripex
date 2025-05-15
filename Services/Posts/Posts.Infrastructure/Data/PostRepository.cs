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

    public PostRepository()
    {
        var session = CassandraSession.Connect();
        _posts = new Table<PostDb>(session);
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
        var posts = await _posts
            .Where(p => p.ProfileId == profileId.Value)
            .ExecuteAsync(); 

        foreach (var post in posts)
        {
            await _posts
                .Where(p => p.Id == post.Id)
                .Delete()
                .ExecuteAsync();
        }
    }
}