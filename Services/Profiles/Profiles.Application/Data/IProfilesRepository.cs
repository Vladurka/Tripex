namespace Profiles.Application.Data;

public interface IProfilesRepository
{
    public Task AddAsync(Profile entity);
    public IQueryable<Profile> GetQueryable();
    public Task<Profile?> GetByIdAsync(Guid id);
    public Task UpdateAsync(Profile entity);
    public Task RemoveAsync(Guid id);
}