using BuildingBlocks.AzureBlob;
using Posts.Application.Data;

namespace Posts.Application.Posts.Commands.CreatePost;

public class CreatePostHandler(IPostRepository repo, IBlobStorageService blobStorageService) 
    : ICommandHandler<CreatePostCommand, CreatePostResult>
{
    public async Task<CreatePostResult> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        var url = await blobStorageService.UploadPhotoAsync(command.Photo, command.ProfileId, cancellationToken);
        
        var post = new PostDb()
        {
            Id = Guid.NewGuid(),
            ProfileId = command.ProfileId,
            ContentUrl = url,
            Description = command.Description
        };
        
        await repo.AddAsync(post);
        
        return new CreatePostResult(post.Id);
    }
}