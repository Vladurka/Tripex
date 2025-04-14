using Microsoft.EntityFrameworkCore.Storage;

namespace Auth.API.Data.Interfaces;

public interface IUsersRepository
{
    public Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    public Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    public Task AddUserAsync(User user, CancellationToken cancellationToken);
    public Task UpdateAsync(User user, CancellationToken cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    public Task<bool> UsernameExistsAsync(string userName, CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}