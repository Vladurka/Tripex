using BuildingBlocks.Exceptions;
using Profiles.Application.Profiles.Queries;

namespace Profiles.Application.Profiles.Commands.UpdateProfile;

public class UpdateProfileHandler(
    IProfilesRepository repo, IOutboxRepository outboxRepo) 
    : ICommandHandler<UpdateProfileCommand, GetProfileResult>
{
    public async Task<GetProfileResult> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await repo.BeginTransactionAsync();
        try
        {
            var profile = await repo.GetByIdAsync(command.ProfileId, false) ??
                throw new NotFoundException("Profile", command.ProfileId);

            profile.Update(command.FirstName, 
                command.LastName, command.Description);
            
            await repo.SaveChangesAsync(false);

            if (!string.IsNullOrWhiteSpace(command.ProfileName) && profile.ProfileName.Value != command.ProfileName)
            {
                if (await repo.ProfileNameExistsAsync(command.ProfileName))
                    throw new ExistsException(command.ProfileName);

                profile.UpdateProfileName(ProfileName.Of(command.ProfileName));
                profile.LastModified = DateTime.UtcNow;
                await repo.SaveChangesAsync();

                var eventMessage = command.Adapt<UpdateUserNameEvent>();

                var outboxMessage = new OutboxMessage(typeof(UpdateUserNameEvent).AssemblyQualifiedName!,
                    JsonSerializer.Serialize(eventMessage));

                await outboxRepo.AddOutboxMessageAsync(outboxMessage);
            }

            await transaction.CommitAsync(cancellationToken);

            return new GetProfileResult(
                profile.Id.Value,
                profile.ProfileName.Value,
                profile.AvatarUrl,
                profile.FirstName,
                profile.LastName,
                profile.Description
            );
        }
        
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
