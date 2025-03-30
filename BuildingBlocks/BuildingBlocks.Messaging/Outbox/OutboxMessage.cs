namespace BuildingBlocks.Messaging.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();        
    public string Type { get; set; } = null!;              
    public string Payload { get; set; } = null!;          
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    
    public OutboxMessage(string type, string payload)
    {
        Type = type;
        Payload = payload;
        CreatedAt = DateTime.UtcNow;
        IsPublished = false;
    }

    protected OutboxMessage() { }
}
