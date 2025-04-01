using BuildingBlocks.Messaging.Outbox;

namespace Posts.Infrastructure.Data;

public class PostsContext(DbContextOptions<PostsContext> options) : DbContext(options), IOutboxContext
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
}