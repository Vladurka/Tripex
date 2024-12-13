using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tripex.Core.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        public string ContentUrl { get; set; } = string.Empty;
    }
}
