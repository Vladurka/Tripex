using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Users
{
    public class UserGetMin : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public UserGetMin(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null");

            Id = user.Id;
            CreatedAt = user.CreatedAt;
            UserName = user.UserName;
            Avatar = user.AvatarUrl;
        }

    }
}
