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
    
    public async Task SaveAsync(Post post)
    {
        var postDb = PostMapper.ToDb(post);
        await _posts.Insert(postDb).ExecuteAsync();
    }

    public async Task<Post?> GetByIdAsync(PostId id)
    {
        var result = await _posts
            .Where(p => p.Id == id.Value)
            .ExecuteAsync();

        var db = result.FirstOrDefault();
        return db?.ToDomain();
    }

    public async Task<IEnumerable<Post>> GetAllByUserAsync(ProfileId id)
    { 
        var result = await _posts
            .Where(p => p.Id == id.Value)
            .ExecuteAsync();

        return result.Select(PostMapper.ToDomain);
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        var result = await _posts.Select(_ => _).ExecuteAsync();
        return result.Select(PostMapper.ToDomain).ToList();
    }

    public async Task DeleteAsync(PostId id)
    {
        await _posts
            .Where(p => p.Id == id.Value)
            .Delete()
            .ExecuteAsync();
    }
}