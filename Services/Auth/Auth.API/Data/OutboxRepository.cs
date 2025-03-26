namespace Auth.API.Data;

public class OutboxRepository(AuthContext context) : IOutboxRepository
{
    public async Task AddOutboxMessageAsync(OutboxMessage message)
    {
        await context.OutboxMessages.AddAsync(message);
        await context.SaveChangesAsync();
    }
}