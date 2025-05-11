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

        await repo.AddPostAsync(post);

        var profileId = ProfileId.Of(command.ProfileId);

        if (await redisRepo.ArePostsCachedAsync(profileId))
            await redisRepo.AddPostAsync(post.ToDomain());

        await repo.IncrementPostCount(profileId);

        var eventMessage = command.Adapt<CacheBasicInfoEvent>();

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        return new CreatePostResult(post.Id);
    }
}
