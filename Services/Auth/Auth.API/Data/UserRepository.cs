using Auth.API.Entities;
using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Auth.API.Data;

public class UserRepository(AuthContext context) : IUsersRepository
{
    public async Task<User?> GetUserByEmailAsync(string email) =>
        await context.Users.FirstOrDefaultAsync(x => x.Email == email);
    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken) =>
        await context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
    
    public async Task<User?> GetUserByIdAsync(Guid id) =>
        await context.Users.FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddUserAsync(User userRegister)
    {
        await context.Users.AddAsync(userRegister);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await GetUserByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User", id);

        context.Users.Remove(user);
        await SaveChangesAsync();
    }
    
    public async Task UpdateAsync(User entity)
    {
        if (await GetUserByIdAsync(entity.Id) == null)
            throw new NotFoundException(entity, entity.Id);

        context.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task<bool> UsernameExistsAsync(string userName) =>
        await context.Users.AnyAsync(x => x.UserName == userName);
    
    public async Task<IDbContextTransaction> BeginTransactionAsync() =>
        await context.Database.BeginTransactionAsync();

    public async Task SaveChangesAsync()
    {
        if (await context.SaveChangesAsync() <= 0)
            throw new InvalidOperationException("Could not save changes");
    }
}