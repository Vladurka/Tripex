using Posts.Application.Posts.Queries;
using Posts.Domain.Models;
using Posts.Domain.ValueObjects;

namespace Posts.Application.Posts.Extensions;

public static class PostMapper
{
    public static PostByIdDb ToDbById(this Post post)
    {
        return new PostByIdDb
        {
            Id = post.Id.Value,
            ProfileId = post.ProfileId.Value,
            ContentUrl = post.ContentUrl.Value,
            Description = post.Description,
            CreatedAt = post.CreatedAt
        };
    }
    
    public static PostByProfileDb ToDbByProfile(this Post post)
    {
        return new PostByProfileDb
        {
            Id = post.Id.Value,
            ProfileId = post.ProfileId.Value,
            ContentUrl = post.ContentUrl.Value,
            Description = post.Description,
            CreatedAt = post.CreatedAt
        };
    }

    public static Post ToDomain(this IPostDb postDb)
    {
        return Post.Create(
            PostId.Of(postDb.Id),
            ProfileId.Of(postDb.ProfileId),
            ContentUrl.Of(postDb.ContentUrl),
            postDb.Description,
            postDb.CreatedAt
        );
    }
    
    public static Post ToDomain(this CachedPostDto db)
    {
        return Post.Create(
            PostId.Of(db.Id),
            ProfileId.Of(db.ProfileId),
            ContentUrl.Of(db.ContentUrl),
            db.Description, db.CreatedAt
        );
    }
    
    public static PostDto ToDto(this Post post)
    {
        return new PostDto(
           post.Id.Value, post.ProfileId.Value, post.ContentUrl.Value,
           post.Description, post.CreatedAt
        );
    }
    
    public static CachedPostDto ToCachedPostDto(this Post post)
    {
        return new CachedPostDto
        {
            Id = post.Id.Value, 
            ProfileId = post.ProfileId.Value, 
            ContentUrl = post.ContentUrl.Value,
            Description = post.Description,
            CreatedAt = post.CreatedAt
        };
    }
}
