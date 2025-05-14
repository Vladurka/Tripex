using BuildingBlocks.Exceptions;
using Posts.Application.Data;
using Posts.Application.Posts.Extensions;

namespace Posts.Application.Posts.Queries.GetPostById;

public class GetPostByIdHandler(IPostRepository repo) 
    : IQueryHandler<GetPostByIdQuery, GetPostByIdResult>
{
    public async Task<GetPostByIdResult> Handle(GetPostByIdQuery query, CancellationToken cancellationToken)
    {
        var post = await repo.GetPostByIdAsync(PostId.Of(query.Id))??
            throw new NotFoundException("Post", query.Id);
                
        return new GetPostByIdResult(post.ToDto());
    }
}