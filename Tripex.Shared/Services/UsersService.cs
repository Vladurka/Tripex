using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Core.Services
{
    public class UsersService(IUsersRepository repo, IPasswordHasher passwordHasher, 
        ICrudRepository<User> crudRepo, ITokenService tokenService, 
        IOptions<JwtOptions> options) : IUsersService
    {
        private JwtOptions _options => options.Value;

        public async Task<ResponseOptions> LoginAsync(User userLogin)
        {
            var user = await repo.GetUserByEmailAsync(userLogin.Email);

            if (user == null || !passwordHasher.VerifyPassword(user.Pass, userLogin.Pass))
                return ResponseOptions.NotFound;

            tokenService.SetTokenWithId(user.Id, _options.TokenName, _options.ExpiresHours);  

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
            var users = await crudRepo.GetQueryable<User>()
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await crudRepo.GetQueryable<User>()
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found");

            return user;
        }
        public async Task<IEnumerable<User>> SearchUsersByNameAsync(string userName)
        {
            var id = tokenService.GetUserIdByToken();

            var users = await crudRepo.GetQueryable<User>()
                .Where(x => EF.Functions.ILike(x.UserName, $"%{userName}%"))
                .Include(u => u.Followers)
                .OrderBy(x => x.Followers.Any(f => f.Id == id))
                .ThenBy(x => x.UserName.Contains(userName) ? 0 : 1)
                .ThenByDescending(x => x.FollowersCount)
                .AsNoTracking()
                .ToListAsync();

            return users;
        }
    }
}
