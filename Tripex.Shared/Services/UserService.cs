using Tripex.Application.DTOs.User;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Security;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Services
{
    public class UserService(IUserRepository repo, IPasswordHasher passwordHasher) : IUserService
    {
        public async Task<ResponseOptions> LoginAsync(UserLogin userLogin)
        {
            var user = await repo.GetUserByEmailAsync(userLogin.Email);

            if (user == null || !passwordHasher.VerifyPassword(user.Pass, userLogin.Pass))
                return ResponseOptions.NotFound;

            return ResponseOptions.Ok;
        }

        public async Task<ResponseOptions> RegisterAsync(UserRegister userRegister)
        {
            var user = await repo.GetUserByEmailAsync(userRegister.Email);

            if (user != null)
                return ResponseOptions.NotFound;

            if (await repo.UsernameExistsAsync(userRegister.UserName))
                return ResponseOptions.Exists;

            string passwordHash = passwordHasher.HashPassword(userRegister.Pass);

            userRegister.Pass = passwordHash;

            await repo.AddUserAsync(userRegister);
            return ResponseOptions.Ok;
        }
    }
}
