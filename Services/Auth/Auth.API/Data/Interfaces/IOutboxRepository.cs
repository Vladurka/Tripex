namespace Auth.API.Data.Interfaces;

public interface IOutboxRepository
{
    public Task AddOutboxMessageAsync(OutboxMessage message);
}