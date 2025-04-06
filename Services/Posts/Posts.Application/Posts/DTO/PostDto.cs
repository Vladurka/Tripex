namespace Posts.Application.Posts.DTO;

public record PostDto(
    Guid Id, Guid ProfileId, 
    string ContentUrl, string? Description,
    DateTime CreatedAt
    );