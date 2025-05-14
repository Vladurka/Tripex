namespace Profiles.Application.Profiles.Commands.DeleteProfile;

public class DeleteProfileHandler(IProfilesRepository repo, IProfilesRedisRepository redisRepo,
    IBlobStorageService blobStorage, IOutboxRepository outboxRepo) 
    : ICommandHandler<DeleteProfileCommand, DeleteProfileResult>
{
    public async Task<DeleteProfileResult> Handle(DeleteProfileCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await repo.BeginTransactionAsync(cancellationToken);
        try
        {
            var profileId = ProfileId.Of(command.ProfileId);
            var profile = await repo.GetProfileByIdAsync(profileId, cancellationToken)
                ?? throw new NotFoundException("Profile", command.ProfileId);

            var eventMessage = command.Adapt<DeleteUserEvent>();

            var outboxMessage = new OutboxMessage(
                typeof(DeleteUserEvent).AssemblyQualifiedName!,
                JsonSerializer.Serialize(eventMessage));

            await repo.RemoveProfileAsync(profile, cancellationToken);
            
            var tasks = new List<Task>
            {
                redisRepo.DeleteBasicInfoAsync(profileId),
                blobStorage.DeletePhotoAsync(profileId.Value, cancellationToken),
                outboxRepo.AddOutboxMessageAsync(outboxMessage)
            };

            if (profile.IsCached)
                tasks.Add(redisRepo.DeleteProfileAsync(profileId));
    
            await Task.WhenAll(tasks);

            await transaction.CommitAsync(cancellationToken);

            return new DeleteProfileResult(true);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}