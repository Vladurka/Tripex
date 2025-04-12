using MediatR;
using Posts.Application.Data;

namespace Posts.Application.Posts.Commands.DeletePostsByProfile;

public class DeletePostsByProfileHandler(IPostsRedisRepository redisRepo, IPostRepository repo) 
    : ICommandHandler<DeletePostsByProfileCommand>
{
    public async Task<Unit> Handle(DeletePostsByProfileCommand command, CancellationToken cancellationToken)
    {
        var profileId = ProfileId.Of(command.ProfileId);
        await repo.DeletePostsAsync(profileId);
        await repo.DeletePostCountAsync(profileId);
        await redisRepo.DeletePostsAsync(profileId);
        
        return Unit.Value;
    }
}