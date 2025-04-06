using BuildingBlocks.AzureBlob;
using Posts.Application.Data;

namespace Posts.Application.Posts.Commands.DeletePost;

public class DeletePostHandler(IPostRepository repo, IBlobStorageService blobStorageService) 
    : ICommandHandler<DeletePostCommand, DeletePostResult>
{
    public async Task<DeletePostResult> Handle(DeletePostCommand command, CancellationToken cancellationToken)
    {
        var posts = await repo.GetPostIdsByUserAsync(ProfileId.Of(command.ProfileId));
        
        if(!posts.Contains(command.PostId))
            throw new InvalidOperationException($"Post {command.PostId} is not your post or doesn't exist");
        
        await repo.DeleteAsync(PostId.Of(command.PostId));
        await blobStorageService.DeletePhotoAsync(command.PostId, cancellationToken);

        return new DeletePostResult(true);
    }
}