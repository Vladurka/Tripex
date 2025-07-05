using Cassandra.Mapping.Attributes;

namespace Posts.Application.Posts.DTO;

[Table("posts_by_id")]
public class PostByIdDb : IPostDb
{
    [PartitionKey]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("profile_id")]
    public Guid ProfileId { get; set; }

    [Column("content_url")]
    public string ContentUrl { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

