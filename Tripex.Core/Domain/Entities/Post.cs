using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tripex.Core.Domain.Interfaces;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Domain.Entities
{
    public class Post : BaseEntity, ILikable
    {
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        [Url]
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }

        public IEnumerable<Like<Post>> Likes { get; set; } = new List<Like<Post>>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<PostWatcher> PostWatchers { get; set; } = new List<PostWatcher>();

        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public int ViewedCount { get; set; } = 0;

        public DateTime ContentUrlUpdated { get; set; } = DateTime.UtcNow;
        public DateTime ViewedCountUpdated { get; set; } = DateTime.UtcNow;

        private const int UPDATE_CONENT_URL_TIME = 590;

        public Post() { }
        public Post(Guid id, Guid userId, string contentUrl, string? description)
        {
            Id = id;
            UserId = userId;
            ContentUrl = contentUrl;
            Description = description;
        }

        public async Task UpdateContentUrlIfNeededAsync(IS3FileService s3FileService, ICrudRepository<Post> repo)
        {
            if (DateTime.UtcNow - ContentUrlUpdated >= TimeSpan.FromMinutes(UPDATE_CONENT_URL_TIME))
            {
                ContentUrl = s3FileService.GetPreSignedURL(Id.ToString(), 10);
                ContentUrlUpdated = DateTime.UtcNow;
                await repo.UpdateAsync(this);
            }
        }

        public async Task UpdateViewedCountAsync(ICrudRepository<Post> repo, ICrudRepository<PostWatcher> postWatcherRepo, Guid postId, Guid userWatched)
        {
            var existingWatcher = await postWatcherRepo.GetQueryable<PostWatcher>()
                .AnyAsync(w => w.UserId == userWatched && w.PostId == postId);

            if (!existingWatcher)
            {
                ViewedCount++;
                await postWatcherRepo.AddAsync(new PostWatcher(userWatched, postId));
                await repo.UpdateAsync(this);
            }
        }
    }
}
