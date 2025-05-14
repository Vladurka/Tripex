using BuildingBlocks.AzureBlob;
using BuildingBlocks.ContentModeration;
using BuildingBlocks.Messaging.Events.Cache;
using Mapster;
using MassTransit;
using Posts.Application.Data;
using Posts.Application.Posts.Extensions;

namespace Posts.Application.Posts.Commands.CreatePost;

public class CreatePostHandler(IPostRepository repo, IBlobStorageService blobStorageService,
    IPostsRedisRepository redisRepo, IPublishEndpoint publishEndpoint,
    IContentModerationService moderationService) : ICommandHandler<CreatePostCommand, CreatePostResult>
{
    public async Task<CreatePostResult> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        
        if (!await moderationService.ModeratePhoto(command.Photo))
            throw new Exception("This photo is not allowed");

        var url = await blobStorageService.UploadPhotoAsync(command.Photo, id, cancellationToken);

        var post = new PostDb
        {
            Id = id,
            ProfileId = command.ProfileId,
            ContentUrl = url,
            Description = command.Description
        };
        
        var eventMessage = command.Adapt<CacheBasicInfoEvent>();
        var profileId = ProfileId.Of(command.ProfileId);
        
        var arePostsCachedTask = redisRepo.ArePostsCachedAsync(profileId);

        var tasks = new List<Task>
        {
            repo.AddPostAsync(post),
            publishEndpoint.Publish(eventMessage, cancellationToken),
            arePostsCachedTask 
        };

        await Task.WhenAll(tasks);

        if (arePostsCachedTask.Result) 
            await redisRepo.AddPostAsync(post.ToDomain());

        return new CreatePostResult(post.Id);
    }
}
