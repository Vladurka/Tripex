namespace Profiles.Application.Data;

public interface IProfilesRepository
{
    public Task AddAsync(Profile entity);
    public IQueryable<Profile> GetQueryable();
    public Task<Profile?> GetByIdAsync(Guid id, bool asNoTracking);
    public Task<bool> ProfileNameExistsAsync(string userName);
    public Task UpdateAsync(Profile entity);
    public Task RemoveAsync(Guid id);
}