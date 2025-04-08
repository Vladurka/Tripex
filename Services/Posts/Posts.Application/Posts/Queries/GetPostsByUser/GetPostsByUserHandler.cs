using Posts.Application.Data;
using Posts.Application.Posts.Extensions;

namespace Posts.Application.Posts.Queries.GetPostsByUser;

public class GetPostsByUserHandler (IPostRepository repo)
    : IQueryHandler<GetPostsByUserQuery, GetPostsByUserResult>
{
    public async Task<GetPostsByUserResult> Handle(GetPostsByUserQuery query, CancellationToken cancellationToken)
    {
        var posts = await repo.GetAllPostsByUserAsync(ProfileId.Of(query.ProfileId));
        return new GetPostsByUserResult(posts.Select(p => p.ToDto()));
    }
        
}