using System.ComponentModel.DataAnnotations;
using Tripex.Core.Services;

namespace Tripex.Core.Domain.Entities
{
    public class User : BaseEntity
    {
        private const string DEFAULT_AVATAR = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS5CQxdTYvVk0IxK9JjTg3YaEPXKfuPfCK3mg&s";
        public string? AvatarUrl { get; set; } = DEFAULT_AVATAR;

        [StringLength(25)]
        public string UserName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int FollowersCount { get; set; } = 0;
        public int FollowingCount { get; set; } = 0;
        public int PostsCount { get; set; } = 0;
        public int ViewedCount { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
        public string Pass { get; set; } = string.Empty;

        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public DateTime AvatarUpdated { get; set; } = DateTime.UtcNow;
        public DateTime ViewedCountUpdated { get; set; } = DateTime.UtcNow;

        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public IEnumerable<Like<Post>> PostLikes { get; set; } = new List<Like<Post>>();
        public IEnumerable<Like<Comment>> CommentLikes { get; set; } = new List<Like<Comment>>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<Follower> Followers { get; set; } = new List<Follower>();
        public IEnumerable<Follower> Following { get; set; } = new List<Follower>();
        public IEnumerable<Watcher<Post>> PostWatchers { get; set; } = new List<Watcher<Post>>();

        private const int UPDATE_AVATAR_URL_TIME = 590;
        private const int VIEW_COUNT_UPDATE_TIME = 1;

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
                if (DateTime.UtcNow - AvatarUpdated >= TimeSpan.FromMinutes(UPDATE_AVATAR_URL_TIME))
                {
                    AvatarUrl = await s3FileService.GetPreSignedURL(Id.ToString(), 10);
                    AvatarUpdated = DateTime.UtcNow;
                    await repo.UpdateAsync(this);
                }
            }
        }

        public async Task UpdateUserIfNeededAsync(ICrudRepository<User> repo, IS3FileService s3FileService)
        {
            if (AvatarUrl != DEFAULT_AVATAR)
            {
                if (DateTime.UtcNow - AvatarUpdated >= TimeSpan.FromMinutes(UPDATE_AVATAR_URL_TIME))
                {
                    AvatarUrl = await s3FileService.GetPreSignedURL(Id.ToString(), 10);
                    AvatarUpdated = DateTime.UtcNow;
                }
            }

            if (DateTime.UtcNow - ViewedCountUpdated >= TimeSpan.FromDays(VIEW_COUNT_UPDATE_TIME))
            {
                ViewedCountUpdated = DateTime.UtcNow;
                ViewedCount = 0;
            }

            ViewedCount++;
            await repo.UpdateAsync(this);
        }
    }
}

