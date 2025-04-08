namespace Posts.Application.Posts.Extensions;

public static class PostCountMapper
{
    public static PostCountDb ToDb(this PostCount post)
    {
        return new PostCountDb
        {
            ProfileId = post.ProfileId.Value,
            Count = post.Count,
        };
    }

    public static PostCount ToDomain(this PostCountDb db)
    {
        return PostCount.Create(
            ProfileId.Of(db.ProfileId)
        );
    }
    
    public static PostCountDto ToDto(this PostCount post) =>
        new PostCountDto(post.ProfileId.Value, post.Count);
}