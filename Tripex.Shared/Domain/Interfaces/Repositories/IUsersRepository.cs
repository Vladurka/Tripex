using Tripex.Core.Domain.Entities;
using Tripex.Core.Enums;

namespace Tripex.Core.Domain.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<ResponseOptions> AddUserAsync(User user);
        public Task<bool> UsernameExistsAsync(string userName);
    }
}
