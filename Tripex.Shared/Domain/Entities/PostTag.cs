using System.ComponentModel.DataAnnotations;

namespace Tripex.Core.Domain.Entities
{
    public class PostTag : BaseEntity
    {
        [Required]
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        [Required]
        public string Tag { get; set; } = string.Empty;
    }
}
