using Posts.Application.Data;

namespace Posts.Application.Posts.Commands.CachePosts;

public class CachePostsHandler(IPostsRedisRepository redisRepo, IPostRepository repo) 
    : ICommandHandler<CachePostsCommand, CachePostsResult>
{
    public async Task<CachePostsResult> Handle(CachePostsCommand command, CancellationToken cancellationToken)
    {
        var profileId = ProfileId.Of(command.ProfileId);
        
        var posts = await repo.GetPostsByUserAsync(profileId);
        
        await redisRepo.CachePostsAsync(posts, profileId);
        
        return new CachePostsResult(true);
    }
}