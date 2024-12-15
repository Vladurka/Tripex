using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tripex.Core.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [JsonPropertyOrder(-3)]
        public Guid Id { get; set; } = new Guid();

        [JsonPropertyOrder(-2)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyOrder(-1)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
