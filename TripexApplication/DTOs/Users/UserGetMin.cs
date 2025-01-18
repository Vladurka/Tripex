namespace Tripex.Application.DTOs.Users
{
    public class UserGetMin
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string? Avatar { get; set; }

        public string CreatedAt { get; set; } = string.Empty;
        public UserGetMin(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Avatar = user.AvatarUrl;

            CreatedAt = user.CreatedAt.Humanize();
        }

        public UserGetMin(Follower user)
        {
            Id = user.Id;
            CreatedAt = user.CreatedAt.Humanize();
            UserName = user.FollowerEntity!.UserName;
            Avatar = user.FollowerEntity.AvatarUrl;
        }
    }
}
