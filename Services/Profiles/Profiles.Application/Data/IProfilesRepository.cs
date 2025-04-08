namespace Profiles.Application.Data;
using Microsoft.EntityFrameworkCore.Storage;

public interface IProfilesRepository
{
    public Task CreateProfileAsync(Profile entity);
    public IQueryable<Profile> GetQueryable();
    public Task<Profile?> GetProfileByIdAsync(Guid id, bool asNoTracking = true);
    public Task<bool> ProfileNameExistsAsync(string userName);
    public Task RemoveProfileAsync(Guid id);
    public Task RemoveProfileAsync(Profile profile);
    public Task<IDbContextTransaction> BeginTransactionAsync();
    public Task SaveChangesAsync(bool shouldUpdate = true);
}