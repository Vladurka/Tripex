using Tripex.Application.DTOs.User;
using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        public Task<ResponseOptions> LoginAsync(UserLogin userLogin);
        public Task<ResponseOptions> RegisterAsync(UserRegister userRegister);
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<User> GetUserInfoByIdAsync(Guid id);
        public Task<User> GetUserByIdAsync(Guid id);
    }
}
