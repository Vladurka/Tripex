using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;

namespace Tripex.Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : IUsersRepository
    {
        public async Task<User?> GetUserByEmailAsync(string email) =>
            await context.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<ResponseOptions> AddUserAsync(User userRegister)
        {
            await context.Users.AddAsync(userRegister);
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
