using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Messaging.Outbox;

public interface IOutboxContext
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
}