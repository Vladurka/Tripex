using Microsoft.Extensions.Options;

namespace Tripex.Core.Services
{
    public class UsersService(IUsersRepository repo, IPasswordHasher passwordHasher, 
        ICrudRepository<User> crudRepo, ITokenService tokenService, 
        IOptions<JwtOptions> options, IS3FileService s3FileService,
        IEmailService emailService) : IUsersService
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
            await using var transaction = await crudRepo.BeginTransactionAsync();
            try
            {
                var user = await repo.GetUserByEmailAsync(userRegister.Email);

                if (user != null)
                    return ResponseOptions.Exists;

                if (await repo.UsernameExistsAsync(userRegister.UserName))
                    return ResponseOptions.Exists;

                string passwordHash = passwordHasher.HashPassword(userRegister.Pass);
                userRegister.Pass = passwordHash;

                await repo.AddUserAsync(userRegister);
                await emailService.SendEmailAsync(userRegister.Email, "Welcome", emailService.WelcomeMessage(userRegister));

                await transaction.CommitAsync();

                return ResponseOptions.Ok;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; 
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await crudRepo.GetQueryable<User>()
                .AsNoTracking()
                .ToListAsync();

            var tasks = users.Select(user => user.UpdateAvatarUrlIfNeededAsync(s3FileService, crudRepo));
            await Task.WhenAll(tasks);

            return users;
        }

        public async Task<IEnumerable<User>> SearchUsersByNameAsync(string userName)
        {
            var id = tokenService.GetUserIdByToken();

            var users = await crudRepo.GetQueryable<User>()
                .AsNoTracking()
                .Where(x => EF.Functions.ILike(x.UserName, $"%{userName}%"))
                .Include(u => u.Followers)
                .OrderBy(x => x.Followers.Any(f => f.Id == id))
                .ThenBy(x => x.UserName.Contains(userName) ? 0 : 1)
                .ThenByDescending(x => x.FollowersCount)
                .AsNoTracking()
                .ToListAsync();

            var tasks = users.Select(user => user.UpdateAvatarUrlIfNeededAsync(s3FileService, crudRepo));
            await Task.WhenAll(tasks);

            return users;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await crudRepo.GetByIdAsync(id);

            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found");

            await Task.WhenAll(
                user.UpdateUserIfNeededAsync(crudRepo, s3FileService)
                );

            return user;
        }
    }
}
