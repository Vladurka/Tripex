using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<ResponseOptions> AddUserAsync(User user);
        public Task<bool> UsernameExistsAsync(string userName);
    }
}
