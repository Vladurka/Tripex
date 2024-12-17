using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Core.Services
{
    public class UsersService(IUsersRepository repo, IPasswordHasher passwordHasher, ICrudRepository<User> crudUserRepo, IPostsService postsService) : IUsersService
    {
        public async Task<ResponseOptions> LoginAsync(User userLogin)
        {
            var user = await repo.GetUserByEmailAsync(userLogin.Email);

            if (user == null || !passwordHasher.VerifyPassword(user.Pass, userLogin.Pass))
                return ResponseOptions.NotFound;

            return ResponseOptions.Ok;
        }

        public async Task<ResponseOptions> RegisterAsync(User userRegister)
        {
            var user = await repo.GetUserByEmailAsync(userRegister.Email);

            if (user != null)
                return ResponseOptions.Exists;

            if (await repo.UsernameExistsAsync(userRegister.UserName))
                return ResponseOptions.Exists;

            string passwordHash = passwordHasher.HashPassword(userRegister.Pass);

            userRegister.Pass = passwordHash;

            await repo.AddUserAsync(userRegister);
            return ResponseOptions.Ok;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await crudUserRepo.GetQueryable<User>()
               .Include(user => user.Posts)
               .ToListAsync();

            return users;
        }

        public async Task<User> GetUserInfoByIdAsync(Guid id)
        {
            var user = await crudUserRepo.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found");

            user.Posts = await postsService.GetPostsByUserIdAsync(user.Id);

            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await crudUserRepo.GetQueryable<User>() 
                .AsNoTracking()                                 
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found");

            return user;
        }
    }
}
