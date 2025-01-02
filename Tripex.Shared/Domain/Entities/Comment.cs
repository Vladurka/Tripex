using System.ComponentModel.DataAnnotations;

namespace Tripex.Core.Domain.Entities
{
    public class Comment : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;
        public int LikesCount { get; set; } = 0;

        public Comment() { }
        public Comment(Guid userId, Guid postId, string content)
        {
            UserId = userId;
            PostId = postId;
            Content = content;
        }
    }
}
