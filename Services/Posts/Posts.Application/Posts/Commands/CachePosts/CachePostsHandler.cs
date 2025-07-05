using MediatR;
using Posts.Application.Data;

namespace Posts.Application.Posts.Commands.CachePosts;

public class CachePostsHandler(IPostsRedisRepository redisRepo, IPostRepository repo) 
    : ICommandHandler<CachePostsCommand>
{
    public async Task<Unit> Handle(CachePostsCommand command, CancellationToken cancellationToken)
    {
        var profileId = ProfileId.Of(command.ProfileId);
        
        var posts = await repo.GetPostsByProfileAsync(profileId);
        
        await redisRepo.CachePostsAsync(posts, profileId);
        
        return Unit.Value;
    }
}