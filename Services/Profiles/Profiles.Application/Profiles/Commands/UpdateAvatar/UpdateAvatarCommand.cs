using Microsoft.AspNetCore.Http;

namespace Profiles.Application.Profiles.Commands.UpdateAvatar;

public record UpdateAvatarCommand(Guid UserId, IFormFile Avatar) : ICommand<UpdateAvatarResult>;
public record UpdateAvatarResult(bool Succeed);
public class UpdateAvatarCommandValidator : AbstractValidator<UpdateAvatarCommand>
{
    public UpdateAvatarCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Avatar).NotEmpty();
    }
}
