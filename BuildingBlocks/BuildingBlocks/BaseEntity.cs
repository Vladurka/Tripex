using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}