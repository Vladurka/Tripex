﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Core.Services
{
    public class UsersService(IUsersRepository repo, IPasswordHasher passwordHasher, 
        ICrudRepository<User> crudUserRepo, ITokenService tokenService, 
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
            var users = await crudUserRepo.GetQueryable<User>()
               .Include(u => u.Posts)
                   .ThenInclude(p => p.Comments)
                       .ThenInclude(p => p.User)
                .Include(u => u.Posts)
                   .ThenInclude(p => p.Likes)
                       .ThenInclude(p => p.User)
               .Include(u => u.Followers)
               .Include(u => u.Following)
               .ToListAsync();

            return users;
        }

        public async Task<IEnumerable<User>> GetUsersInfoByNameAsync(string userName)
        {
            var users = await crudUserRepo.GetQueryable<User>()
                .Where(x => EF.Functions.ILike(x.UserName, $"%{userName}%"))
                .Include(u => u.Posts)
                   .ThenInclude(p => p.Comments)
                       .ThenInclude(p => p.User)
                .Include(u => u.Posts)
                   .ThenInclude(p => p.Likes)
                       .ThenInclude(p => p.User)
               .Include(u => u.Followers)
               .Include(u => u.Following)
               .ToListAsync();
            return users;
        }

    }
}
