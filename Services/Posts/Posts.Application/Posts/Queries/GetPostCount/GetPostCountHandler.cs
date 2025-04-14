using Posts.Application.Data;

namespace Posts.Application.Posts.Queries.GetPostCount;

public class GetPostCountHandler(IPostRepository repo) : IQueryHandler<GetPostCountQuery, GetPostCountResult>
{
    public async Task<GetPostCountResult> Handle(GetPostCountQuery query, CancellationToken cancellationToken) =>
        new GetPostCountResult(await repo.GetPostCount(ProfileId.Of(query.ProfileId)));
}