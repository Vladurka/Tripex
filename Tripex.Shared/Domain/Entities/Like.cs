using System.ComponentModel.DataAnnotations.Schema;

namespace Tripex.Core.Domain.Entities
{
    public class Like : BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [ForeignKey("Post")]
        public Guid PostId { get; set; }
    }
}
