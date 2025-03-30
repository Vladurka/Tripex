using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Messaging.Outbox;

public interface IOutboxRepository
{
    public Task AddOutboxMessageAsync(OutboxMessage message);
}