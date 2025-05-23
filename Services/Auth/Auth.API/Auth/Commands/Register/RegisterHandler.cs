using System.Text.Json;
using Mapster;

namespace Auth.API.Auth.Commands.Register;

public class RegisterHandler(IPasswordHasher passwordHasher, ITokenService tokenService, 
    IOptions<JwtOptions> options, IUsersRepository repo, 
    IOutboxRepository outboxRepo) : ICommandHandler<RegisterCommand, RegisterResult>
{
    private JwtOptions _options => options.Value;
    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await repo.BeginTransactionAsync(cancellationToken);
        try
        {
            var userExists = await repo.UserExists(command.Email, cancellationToken);
            
            if (userExists)
                throw new ExistsException("User", command.Email);

            if (await repo.UsernameExistsAsync(command.UserName, cancellationToken))
                throw new ExistsException("User", command.UserName);
            
            var user = new User(command.UserName, command.Email, passwordHasher.HashPassword(command.Password));
            
            var tokens = tokenService.GenerateTokens(user.Id);
            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays);

            await repo.AddUserAsync(user, cancellationToken);

            var eventMessage = command.Adapt<CreateProfileEvent>();
            eventMessage.UserId = user.Id;

            var outboxMessage = new OutboxMessage(typeof(CreateProfileEvent).AssemblyQualifiedName!,
                JsonSerializer.Serialize(eventMessage));
            
            await outboxRepo.AddOutboxMessageAsync(outboxMessage);

            await transaction.CommitAsync(cancellationToken);
            
            tokenService.SetTokenWithId(user.Id, _options.TokenName, _options.AccessTokenExpirationMinutes);

            return new RegisterResult(user.Id); 
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}