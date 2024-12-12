using System.ComponentModel.DataAnnotations;

namespace Tripex.Core.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public DateTime PublishingDate { get; set; } = DateTime.UtcNow;
    }
}
