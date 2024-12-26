using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        public Task<ResponseOptions> LoginAsync(User userLogin);
        public Task<ResponseOptions> RegisterAsync(User userRegister);
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<IEnumerable<User>> SearchUsersByNameAsync(string userName, int pageIndex, int pageSize = 20);
        public Task<User> GetUserByIdAsync(Guid id);
    }
}
