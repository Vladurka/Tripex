using Tripex.Application.DTOs.Posts;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Users
{
    public class UserGet : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public IEnumerable<PostGet> Posts { get; set; } = new List<PostGet>();
        public IEnumerable<UserGetMin> Followers { get; set; } = new List<UserGetMin>();
        public IEnumerable<UserGetMin> Following { get; set; } = new List<UserGetMin>();

        public int FollowersCount {  get; set; }
        public int FollowingCount { get; set; }
        public UserGet(User user)
        {
            Id = user.Id;
            CreatedAt = user.CreatedAt;
            UserName = user.UserName;
            Avatar = user.AvatarUrl;

            Posts = user.Posts
                .Select(post => new PostGet(post))
                .ToList();

            Following = user.Followers
                .Select(follower => new UserGetMin(follower.FollowingEntity))
                .ToList();

            Followers = user.Following
                .Select(following => new UserGetMin(following.FollowerEntity))
                .ToList();

            FollowersCount = Followers.Count();
            FollowingCount = Following.Count();
        }
    }
}
