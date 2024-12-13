using Microsoft.EntityFrameworkCore;
using Tripex.Application.DTOs.User;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;

namespace Tripex.Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : IUsersRepository
    {
        public async Task<User?> GetUserByEmailAsync(string email) =>
            await context.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<ResponseOptions> AddUserAsync(UserRegister userRegister)
        {
            var user = new User(userRegister.UserName, userRegister.Email, userRegister.Pass);

            await context.Users.AddAsync(user);
            await SaveChangesAsync();

            return ResponseOptions.Ok;
        }

        public async Task<bool> UsernameExistsAsync(string userName) =>
            await context.Users.AnyAsync(x => x.UserName == userName);

        private async Task SaveChangesAsync()
        {
            if (await context.SaveChangesAsync() <= 0)
                throw new InvalidOperationException("Could not save changes");
        }
    }
}
