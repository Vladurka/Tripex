namespace Posts.Application.Posts.Queries.GetPosts;

public record GetPostsQuery() : IQuery<GetPostsResult>;
public record GetPostsResult(IEnumerable<PostDto> Posts);