using Posts.Application.Data;
using Posts.Application.Posts.Extensions;

namespace Posts.Application.Posts.Queries.GetPosts;

public class GetPostsHandler(IPostRepository repo) 
    : IQueryHandler<GetPostsQuery, GetPostsResult>
{
    public async Task<GetPostsResult> Handle(GetPostsQuery query, CancellationToken cancellationToken)
    {
        var posts = await repo.GetAllPostsAsync();
        return new GetPostsResult(posts.Select(x => x.ToDto()));
    }
}