using System.ComponentModel.DataAnnotations;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Domain.Entities
{
    public class User : BaseEntity
    {
        private const string DEFAULT_AVATAR = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS5CQxdTYvVk0IxK9JjTg3YaEPXKfuPfCK3mg&s";

        [Url]
        public string? AvatarUrl { get; set; } = DEFAULT_AVATAR;

        [StringLength(25, MinimumLength = 2)]
        public string UserName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int FollowersCount { get; set; } = 0;
        public int FollowingCount { get; set; } = 0;
        public int PostsCount { get; set; } = 0;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MinLength(8)]
        public string Pass { get; set; } = string.Empty;

        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public DateTime AvatarUpdated { get; set; } = DateTime.UtcNow;

        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public IEnumerable<Like> Likes { get; set; } = new List<Like>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<Follower> Followers { get; set; } = new List<Follower>();
        public IEnumerable<Follower> Following { get; set; } = new List<Follower>();

        public User() { }
        public User(string userName, string email, string pass)
        {
            UserName = userName;
            Email = email;
            Pass = pass;
        }
        public User(string email, string pass)
        {
            Email = email;
            Pass = pass;
        }

        public async Task UpdateAvatarUrlIfNeededAsync(IS3FileService s3FileService, ICrudRepository<User> repo)
        {
            if (AvatarUrl != DEFAULT_AVATAR)
            {
                if (DateTime.UtcNow - AvatarUpdated >= TimeSpan.FromMinutes(590))
                {
                    AvatarUrl = s3FileService.GetPreSignedURL(Id.ToString(), 10);
                    AvatarUpdated = DateTime.UtcNow;
                    await repo.UpdateAsync(this);
                }
            }
        }
    }
}

