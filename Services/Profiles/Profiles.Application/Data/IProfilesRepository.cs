namespace Profiles.Application.Data;
using Microsoft.EntityFrameworkCore.Storage;

public interface IProfilesRepository
{
    public Task AddAsync(Profile entity);
    public IQueryable<Profile> GetQueryable();
    public Task<Profile?> GetByIdAsync(Guid id, bool asNoTracking = true);
    public Task<bool> ProfileNameExistsAsync(string userName);
    public Task RemoveAsync(Guid id);
    public Task<IDbContextTransaction> BeginTransactionAsync();
    public Task SaveChangesAsync(bool shouldUpdate = true);
}