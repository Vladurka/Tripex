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

        var postId = PostId.Of(command.PostId);
        
        await repo.DeletePostAsync(postId);
        await redisRepo.DeletePostAsync(postId, ProfileId.Of(command.ProfileId));
        await blobStorageService.DeletePhotoAsync(postId.Value, cancellationToken);

        return new DeletePostResult(true);
    }
}