namespace Auth.API.Auth.Commands.RefreshToken;

public class RefreshTokenHandler( ITokenService tokenService, IOptions<JwtOptions> options, 
    IUsersRepository repo) : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private JwtOptions _options => options.Value;
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var user = await repo.GetUserByRefreshTokenAsync(command.RefreshToken);

        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new Exception("Invalid or expired refresh token");
        
        var tokens = tokenService.GenerateTokens(user.Id);

        tokenService.SetTokenWithId(user.Id, _options.TokenName, _options.AccessTokenExpirationMinutes);
        
        user.RefreshToken = tokens.RefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays);
        await repo.UpdateAsync(user);

        return new RefreshTokenResult(tokens.RefreshToken);
    }
}