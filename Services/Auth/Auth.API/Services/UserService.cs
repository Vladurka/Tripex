using Auth.API.Entities;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events.Profiles;
using MassTransit;
using Mapster;

namespace Auth.API.Services;
public class UserService(IPasswordHasher passwordHasher, ITokenService tokenService, 
    IOptions<JwtOptions> options, IUsersRepository repo, IPublishEndpoint publishEndpoint) : IUserService
{
    private JwtOptions _options => options.Value;

    public async Task LoginAsync(LoginDto dto)
    {
        var user = await repo.GetUserByEmailAsync(dto.Email);

        if (user == null)
           throw new NotFoundException(dto, dto.Email);
        
        if (!passwordHasher.VerifyPassword(user.Password, dto.Password))
            throw new Exception("Bad password");

        tokenService.SetTokenWithId(user.Id, _options.TokenName, _options.ExpiresHours);  
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        await using var transaction = await repo.BeginTransactionAsync();
        try
        {
            var userCheck = await repo.GetUserByEmailAsync(dto.Email);

            if (userCheck != null)
                throw new ExistsException(dto, dto.Email);

            if(await repo.UsernameExistsAsync(dto.UserName))
                throw new ExistsException(dto, dto.UserName);
            
            dto.Password = passwordHasher.HashPassword(dto.Password);

            var user = new User(dto.UserName, dto.Email, dto.Password);
            
            await repo.AddUserAsync(user);

            var eventMessage = dto.Adapt<CreateProfileEvent>();

            await publishEndpoint.Publish(eventMessage);
            
            tokenService.SetTokenWithId(user.Id, _options.TokenName, _options.ExpiresHours);  

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}