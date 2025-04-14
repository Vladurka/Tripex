namespace Profiles.Application.Data;
using Microsoft.EntityFrameworkCore.Storage;

public interface IProfilesRepository
{
    public Task CreateProfileAsync(Profile profile, CancellationToken cancellationToken);
    public IQueryable<Profile> GetQueryable();
    public Task<Profile?> GetProfileByIdAsync(Guid id, CancellationToken cancellationToken, bool asNoTracking = true);
    public Task<bool> ProfileNameExistsAsync(string userName, CancellationToken cancellationToken);
    public Task RemoveProfileAsync(Guid id, CancellationToken cancellationToken);
    public Task RemoveProfileAsync(Profile profile, CancellationToken cancellationToken);
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken, bool shouldUpdate = true);
}