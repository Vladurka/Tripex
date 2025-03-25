namespace Profiles.Application.Profiles.Queries;

public record GetProfileResult(
    string UserName,
    string AvatarUrl,
    string? FirstName,
    string? LastName,
    string? Description);