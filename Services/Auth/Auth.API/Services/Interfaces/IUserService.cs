using Auth.API.Entities;

namespace Auth.API.Services.Interfaces;

public interface IUserService
{
    public Task<TokenModel> LoginAsync(LoginDto userLogin);
    public Task<TokenModel> RegisterAsync(RegisterDto userRegister);
    public Task<TokenModel> RefreshAsync(string refreshToken);
}