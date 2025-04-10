using BuildingBlocks.AzureBlob;
using Posts.Application.Data;
using Posts.Application.Posts.Extensions;

namespace Posts.Application.Posts.Commands.CreatePost;

public class CreatePostHandler(IPostRepository repo, IBlobStorageService blobStorageService,
    IPostsRedisRepository redisRepo) : ICommandHandler<CreatePostCommand, CreatePostResult>
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
        
        await repo.AddPostAsync(post);

        if (await redisRepo.ArePostsCachedAsync(ProfileId.Of(command.ProfileId)))
            await redisRepo.AddPostAsync(post.ToDomain());
        
        await repo.IncrementPostCount(command.ProfileId);
        
        return new CreatePostResult(post.Id);
    }
}