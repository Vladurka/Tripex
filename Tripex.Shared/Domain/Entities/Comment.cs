using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tripex.Core.Domain.Entities
{
    public class Comment : BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Post")]
        public Guid PostId { get; set; }

        [Required]
        public string ContentUrl { get; set; } = string.Empty;
    }
}
