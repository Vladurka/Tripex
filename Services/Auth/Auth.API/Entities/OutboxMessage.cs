namespace Auth.API.Entities;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();        
    public string Type { get; set; } = null!;              
    public string Payload { get; set; } = null!;          
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; } = false;        
    public DateTime? PublishedAt { get; set; } 
}
