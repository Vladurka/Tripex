using BuildingBlocks.Messaging.Events.Profiles;
using MassTransit;
using Mapster;

namespace Auth.API.Services;
public class UserService(IPasswordHasher passwordHasher, ITokenService tokenService, 
    IOptions<JwtOptions> options, IUsersRepository repo, 
    IPublishEndpoint publishEndpoint) : IUserService
{
    private JwtOptions _options => options.Value;

    public async Task<TokenModel> LoginAsync(LoginDto dto)
    {
        var user = await repo.GetUserByEmailAsync(dto.Email);

        if (user == null)
            throw new NotFoundException(dto, dto.Email);

        if (!passwordHasher.VerifyPassword(user.PasswordHash, dto.Password))
            throw new Exception("Bad password");
        
        var tokens = tokenService.GenerateTokens(user.Id);
        
        tokenService.SetTokenWithId(user.Id, _options.TokenName, _options.AccessTokenExpirationMinutes);
        
        user.RefreshToken = tokens.RefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays);
        await repo.UpdateAsync(user); 

        return tokens;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterDto dto)
    {
        await using var transaction = await repo.BeginTransactionAsync();
        try
        {
            var userCheck = await repo.GetUserByEmailAsync(dto.Email);

            if (userCheck != null)
                throw new ExistsException(dto, dto.Email);

            if (await repo.UsernameExistsAsync(dto.UserName))
                throw new ExistsException(dto, dto.UserName);

            dto.Password = passwordHasher.HashPassword(dto.Password);

            var user = new User(dto.UserName, dto.Email, dto.Password);
        
            await repo.AddUserAsync(user);

            var eventMessage = dto.Adapt<CreateProfileEvent>();

            eventMessage.UserName = user.UserName;
            eventMessage.UserId = user.Id;

            await publishEndpoint.Publish(eventMessage);
            
            var tokens = tokenService.GenerateTokens(user.Id);
            
            tokenService.SetTokenWithId(user.Id, _options.TokenName, _options.AccessTokenExpirationMinutes);
            
            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays);
            await repo.UpdateAsync(user);

            await transaction.CommitAsync();

            return new RegisterResponse(tokens, user.Id); 
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<TokenModel> RefreshAsync(string refreshToken)
    {
        var user = await repo.GetUserByRefreshTokenAsync(refreshToken);

        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new Exception("Invalid or expired refresh token");
        
        var tokens = tokenService.GenerateTokens(user.Id);

        tokenService.SetTokenWithId(user.Id, _options.TokenName, _options.AccessTokenExpirationMinutes);
        
        user.RefreshToken = tokens.RefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays);
        await repo.UpdateAsync(user);

        return tokens; 
    }
}