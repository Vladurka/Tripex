using BuildingBlocks.AzureBlob;
using Posts.Application.Data;

namespace Posts.Application.Posts.Commands.DeletePost;

public class DeletePostHandler(IPostRepository repo, IBlobStorageService blobStorageService,
    IPostsRedisRepository redisRepo) : ICommandHandler<DeletePostCommand, DeletePostResult>
{
    public async Task<DeletePostResult> Handle(DeletePostCommand command, CancellationToken cancellationToken)
    {
        var posts = await repo.GetPostIdsByUserAsync(ProfileId.Of(command.ProfileId));
        
        if(!posts.Contains(command.PostId))
            throw new InvalidOperationException($"Post {command.PostId} is not your post or doesn't exist");

        var profileId = ProfileId.Of(command.ProfileId);
        var postId = PostId.Of(command.PostId);
        
        await repo.DeletePostAsync(postId);
        await repo.DecrementPostCount(profileId);
        await redisRepo.DeletePostAsync(postId, profileId);
        await blobStorageService.DeletePhotoAsync(postId.Value, cancellationToken);

        return new DeletePostResult(true);
    }
}