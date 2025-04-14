namespace Auth.API.Auth.Commands.UpdateUserName;

public record UpdateUserNameCommand(Guid UserId, string UserName) : ICommand;

public class UpdateUsernameCommandValidator : AbstractValidator<UpdateUserNameCommand>
{
    public UpdateUsernameCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
    }
}