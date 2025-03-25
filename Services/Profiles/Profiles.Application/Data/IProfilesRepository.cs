namespace Profiles.Application.Data;

public interface IProfilesRepository
{
    public Task AddAsync(Profile entity);
    public IQueryable<Profile> GetQueryable();
    public Task<Profile?> GetByIdAsync(Guid id);
    public Task<Profile?> GetByUserNameAsync(string userName);
    public Task<bool> UsernameExistsAsync(string userName);
    public Task UpdateAsync(Profile entity);
    public Task RemoveAsync(Guid id);
}