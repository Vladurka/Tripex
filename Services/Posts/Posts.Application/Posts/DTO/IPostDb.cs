namespace Posts.Application.Posts.DTO;

public interface IPostDb
{
    public Guid Id { get; set; }
    
    public Guid ProfileId { get; set; }
    
    public string ContentUrl { get; set; } 
    
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
}