using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tripex.Core.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [JsonPropertyOrder(-2)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyOrder(-1)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
