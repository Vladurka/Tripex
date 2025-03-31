using Microsoft.AspNetCore.Http;

namespace Profiles.Application.Profiles.Commands.UpdateAvatar;

public class UpdateAvatarCommand : ICommand<UpdateAvatarResult>
{
    public Guid UserId { get; set; }
    public IFormFile? Avatar { get; set; }
}
public record UpdateAvatarResult(string AvatarUrl);
public class UpdateAvatarCommandValidator : AbstractValidator<UpdateAvatarCommand>
{
    public UpdateAvatarCommandValidator() =>
        RuleFor(x => x.UserId).NotEmpty();
}
