using Tripex.Application.DTOs.User;
using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<ResponseOptions> AddUserAsync(UserRegister user);
        public Task<bool> UsernameExistsAsync(string userName);
    }
}
