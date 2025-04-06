using Cassandra.Mapping.Attributes;

namespace Posts.Application.Posts.DTO;

[Table("post_count")]
public class PostCountDb
{
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [PartitionKey]
    [Column("profile_id")]
    public Guid ProfileId { get; set; }

    [Column("count")]
    public int Count { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}