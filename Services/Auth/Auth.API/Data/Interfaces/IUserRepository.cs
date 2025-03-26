using Auth.API.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Auth.API.Data.Interfaces;

public interface IUsersRepository
{
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<User?> GetUserByIdAsync(Guid id);
    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    public Task AddUserAsync(User user);
    public Task UpdateAsync(User entity);
    public Task DeleteAsync(Guid id);
    public Task<bool> UsernameExistsAsync(string userName);
    public Task SaveChangesAsync();
    public Task<IDbContextTransaction> BeginTransactionAsync();
}