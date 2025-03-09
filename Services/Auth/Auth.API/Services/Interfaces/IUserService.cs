namespace Auth.API.Services.Interfaces;

public interface IUserService
{
    public Task LoginAsync(LoginDto userLogin);
    public Task RegisterAsync(RegisterDto userRegister);
}