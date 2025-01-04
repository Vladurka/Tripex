﻿using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Domain.Entities
{
    public class Post : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        [Url]
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }

        public IEnumerable<Like> Likes { get; set; } = new List<Like>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<PostTag> Tags { get; set; } = new List<PostTag>();

        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public int ViewedCount { get; set; } = 0;

        public DateTime ContentUrlUpdated { get; set; } = DateTime.UtcNow;
        public DateTime ViewedCountUpdated { get; set; } = DateTime.UtcNow;

        private const int UPDATE_CONENT_URL_TIME = 590;
        private const int VIEW_COUNT_UPDATE_TIME = 1;

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

        public async Task UpdateViewedCountAsync(ICrudRepository<Post> repo)
        {
            //throw new Exception($"{ViewedCountUpdated - DateTime.UtcNow}");
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
