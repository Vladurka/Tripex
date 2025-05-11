namespace Auth.API.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator() =>
        RuleFor(x => x.RefreshToken).NotEmpty();
}