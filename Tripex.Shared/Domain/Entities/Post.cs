using System.ComponentModel.DataAnnotations;

namespace Tripex.Core.Domain.Entities
{
    public class Post : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IEnumerable<Like> Likes { get; set; } = new List<Like>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
