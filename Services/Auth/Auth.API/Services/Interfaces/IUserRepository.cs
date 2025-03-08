using Auth.API.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Auth.API.Services.Interfaces;

public interface IUsersRepository
{
    public Task<User?> GetUserByEmailAsync(string email);
    public Task AddUserAsync(User user);
    public Task<bool> UsernameExistsAsync(string userName);
    public Task<IDbContextTransaction> BeginTransactionAsync();
}