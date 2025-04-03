namespace Profiles.Application.Profiles.Commands.DeleteProfile;

public class DeleteProfileHandler(IProfilesRepository repo, 
    IBlobStorageService blobStorage, IOutboxRepository outboxRepo) 
    : ICommandHandler<DeleteProfileCommand, DeleteProfileResult>
{
    public async Task<DeleteProfileResult> Handle(DeleteProfileCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await repo.BeginTransactionAsync();
        try
        {
            await repo.RemoveAsync(command.UserId);
            
            await blobStorage.DeletePhotoAsync(command.UserId, cancellationToken);

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