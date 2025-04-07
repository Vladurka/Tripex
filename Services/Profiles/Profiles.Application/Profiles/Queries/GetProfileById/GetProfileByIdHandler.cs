using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events.Cache;

namespace Profiles.Application.Profiles.Queries.GetProfileById;

public class GetProfileByIdHandler(IProfilesRepository repo, IOutboxRepository outboxRepo,
    IProfilesRedisRepository redisRepo) : IQueryHandler<GetProfileByIdQuery, GetProfileResult>
{
    public async Task<GetProfileResult> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken)
    {
        var profile = await redisRepo.GetCachedProfileAsync(query.ProfileId);
            
        if(profile == null)
            profile = await repo.GetByIdAsync(query.ProfileId, false) ?? 
                      throw new NotFoundException("Profile", query.ProfileId);
        
        if (profile.ShouldBeCached())
        {
            var eventMessage = query.Adapt<CacheProfileEvent>();
            var outboxMessage = new OutboxMessage(typeof(CacheProfileEvent).AssemblyQualifiedName!,
                JsonSerializer.Serialize(eventMessage));
            await outboxRepo.AddOutboxMessageAsync(outboxMessage);
        }
        
        await repo.SaveChangesAsync(false);

        return new GetProfileResult(
            profile.Id.Value,
            profile.ProfileName.Value,
            profile.AvatarUrl,
            profile.FirstName,
            profile.LastName,
            profile.Description
        );
    }
}