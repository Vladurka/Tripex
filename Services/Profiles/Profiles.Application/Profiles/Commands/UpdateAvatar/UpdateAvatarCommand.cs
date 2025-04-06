using Microsoft.AspNetCore.Http;

namespace Profiles.Application.Profiles.Commands.UpdateAvatar;

public class UpdateAvatarCommand : ICommand<UpdateAvatarResult>
{
    public Guid ProfileId { get; set; }
    public IFormFile? Avatar { get; set; }
}
public record UpdateAvatarResult(string AvatarUrl);
