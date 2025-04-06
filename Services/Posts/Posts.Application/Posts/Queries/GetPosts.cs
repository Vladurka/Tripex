namespace Posts.Application.Posts.Queries;

public record PostDto(
    Guid Id, Guid ProfileId, 
    string ContentUrl, string? Description,
    DateTime CreatedAt
    );