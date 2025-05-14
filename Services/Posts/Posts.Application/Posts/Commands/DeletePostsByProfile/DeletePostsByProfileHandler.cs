using MediatR;
using Posts.Application.Data;

namespace Posts.Application.Posts.Commands.DeletePostsByProfile;

public class DeletePostsByProfileHandler(IPostsRedisRepository redisRepo, IPostRepository repo) 
    : ICommandHandler<DeletePostsByProfileCommand>
{
    public async Task<Unit> Handle(DeletePostsByProfileCommand command, CancellationToken cancellationToken)
    {
        var profileId = ProfileId.Of(command.ProfileId);

        await Task.WhenAll(repo.DeletePostsAsync(profileId), 
            repo.DeletePostCountAsync(profileId), 
            redisRepo.DeletePostsByProfileAsync(profileId));
        
        return Unit.Value;
    }
}