using Auth.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Auth.API.Data;

public class UserRepository(AuthContext context) : IUsersRepository
{
    public async Task<User?> GetUserByEmailAsync(string email) =>
        await context.Users.FirstOrDefaultAsync(x => x.Email == email);

    public async Task AddUserAsync(User userRegister)
    {
        await context.Users.AddAsync(userRegister);
        await SaveChangesAsync();
    }

    public async Task<bool> UsernameExistsAsync(string userName) =>
        await context.Users.AnyAsync(x => x.UserName == userName);
    
    public async Task<IDbContextTransaction> BeginTransactionAsync() =>
        await context.Database.BeginTransactionAsync();

    private async Task SaveChangesAsync()
    {
        if (await context.SaveChangesAsync() <= 0)
            throw new InvalidOperationException("Could not save changes");
    }
}