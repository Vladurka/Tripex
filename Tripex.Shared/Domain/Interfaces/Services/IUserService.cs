using Tripex.Application.DTOs.User;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IUserService
    {
        public Task<ResponseOptions> LoginAsync(UserLogin userLogin);
        public Task<ResponseOptions> RegisterAsync(UserRegister userRegister);
    }
}
