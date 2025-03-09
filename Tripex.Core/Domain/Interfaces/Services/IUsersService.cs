namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IUsersService
    {
        public Task<ResponseOptions> LoginAsync(User userLogin);
        public Task<ResponseOptions> RegisterAsync(User userRegister);
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<IEnumerable<User>> SearchUsersByNameAsync(string userName);
        public Task<User> GetUserByIdAsync(Guid id);
    }
}
