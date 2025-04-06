using Posts.Application.Posts.Queries;
using Posts.Domain.Models;
using Posts.Domain.ValueObjects;

namespace Posts.Application.Posts.Extensions;

public static class PostMapper
{
    public static PostDb ToDb(this Post post)
    {
        return new PostDb
        {
            Id = post.Id.Value,
            ProfileId = post.ProfileId.Value,
            ContentUrl = post.ContentUrl.Value,
            Description = post.Description,
            CreatedAt = post.CreatedAt
        };
    }

    public static Post ToDomain(this PostDb db)
    {
        return Post.Create(
            PostId.Of(db.Id),
            ProfileId.Of(db.ProfileId),
            ContentUrl.Of(db.ContentUrl),
            db.Description
        );
    }
    
    public static PostDto ToDto(this Post post)
    {
        return new PostDto(
           post.Id.Value, post.ProfileId.Value, post.ContentUrl.Value,
           post.Description ,post.CreatedAt
        );
    }
}
