using System.ComponentModel.DataAnnotations;

namespace Tripex.Core.Domain.Entities
{
    public class Like : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public Guid PostId { get; set; }
        public Post? Post { get; set; }

        public Like() { }
        public Like(Guid userId, Guid postId)
        {
            UserId = userId;
            PostId = postId;
        }
    }
}
