using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tripex.Core.Domain.Entities
{
    public class Post : BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
