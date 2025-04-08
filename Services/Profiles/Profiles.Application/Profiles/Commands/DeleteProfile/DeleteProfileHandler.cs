namespace Profiles.Application.Profiles.Commands.DeleteProfile;

public class DeleteProfileHandler(IProfilesRepository repo, IProfilesRedisRepository redisRepo,
    IBlobStorageService blobStorage, IOutboxRepository outboxRepo) 
    : ICommandHandler<DeleteProfileCommand, DeleteProfileResult>
{
    public async Task<DeleteProfileResult> Handle(DeleteProfileCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await repo.BeginTransactionAsync();
        try
        {
            var profile = await repo.GetProfileByIdAsync(command.ProfileId);

            if (profile == null)
                throw new NotFoundException("Profile", command.ProfileId);
            
            await repo.RemoveProfileAsync(profile);
            
            if(profile.IsCached)
                await redisRepo.DeleteProfileAsync(command.ProfileId);
            
            await blobStorage.DeletePhotoAsync(command.ProfileId, cancellationToken);

            var eventMessage = command.Adapt<DeleteUserEvent>();
            
            var outboxMessage = new OutboxMessage(typeof(DeleteUserEvent).AssemblyQualifiedName!,
                JsonSerializer.Serialize(eventMessage));
            
            await outboxRepo.AddOutboxMessageAsync(outboxMessage);
            
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