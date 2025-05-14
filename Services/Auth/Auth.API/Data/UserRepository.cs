using Auth.API.Entities;
using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Auth.API.Data;

public class UserRepository(AuthContext context) : IUsersRepository
{
    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken) =>
        await context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken) =>
        await context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);
    
    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddUserAsync(User userRegister, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(userRegister, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await GetUserByIdAsync(id, cancellationToken);

        if (user == null)
            throw new NotFoundException("User", id);

        context.Users.Remove(user);
        await SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        if (await GetUserByIdAsync(user.Id, cancellationToken) == null)
            throw new NotFoundException(user, user.Id);

        context.Entry(user).State = EntityState.Modified;
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UsernameExistsAsync(string userName, CancellationToken cancellationToken) =>
        await context.Users.AnyAsync(x => x.UserName == userName, cancellationToken);

    public async Task<bool> UserExists(string email, CancellationToken cancellationToken) =>
        await context.Users.AnyAsync(x => x.Email == email, cancellationToken);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken) =>
        await context.Database.BeginTransactionAsync(cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        if (await context.SaveChangesAsync(cancellationToken) <= 0)
            throw new InvalidOperationException("Could not save changes");
    }
}