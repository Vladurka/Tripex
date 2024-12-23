using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Users
{
    public class UserGetMin : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public UserGetMin(User user)
        {
            Id = user.Id;
            CreatedAt = user.CreatedAt;
            UserName = user.UserName;
            Avatar = user.AvatarUrl;
        }

        public UserGetMin(Follower user)
        {
            Id = user.Id;
            CreatedAt = user.CreatedAt;
            UserName = user.FollowerEntity.UserName;
            Avatar = user.FollowerEntity.AvatarUrl;
        }

    }
}
