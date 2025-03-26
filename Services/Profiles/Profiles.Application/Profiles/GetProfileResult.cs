namespace Profiles.Application.Profiles.Queries;

public record GetProfileResult(
    Guid Id,
    string ProfileName,
    string AvatarUrl,
    string? FirstName,
    string? LastName,
    string? Description);