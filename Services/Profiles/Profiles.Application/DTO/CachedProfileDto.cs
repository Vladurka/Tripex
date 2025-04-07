namespace Profiles.Application.DTO;

public class CachedProfileDto
{
    public Guid Id { get; set; }
    public string ProfileName { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Description { get; set; }
    
    public CachedProfileDto(){}
}
