using System.ComponentModel.DataAnnotations;

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

        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;

        public Post() { }
        public Post(Guid userId, string contentUrl, string? description)
        {
            UserId = userId;
            ContentUrl = contentUrl;
            Description = description;
        }
    }
}
