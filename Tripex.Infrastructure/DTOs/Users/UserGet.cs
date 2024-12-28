using Humanizer;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Users
{
    public class UserGet
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Description { get; set; }

        public int FollowersCount {  get; set; }
        public int FollowingCount { get; set; }
        public int PostsCount { get; set; }

        public string CreatedAt { get; set; } = string.Empty;
        public UserGet(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Avatar = user.AvatarUrl;
            Description = user.Description;

            FollowersCount = user.FollowersCount;
            FollowingCount = user.FollowingCount;
            PostsCount = user.PostsCount;

            CreatedAt = user.CreatedAt.Humanize();
        }
    }
}
