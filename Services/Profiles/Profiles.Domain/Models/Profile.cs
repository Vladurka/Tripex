using Profiles.Domain.Abstractions;
using Profiles.Domain.ValueObjects;

namespace Profiles.Domain.Models;
public class Profile : Entity<ProfileId>
{
    public const string DEFAULT_AVATAR = "https://shorturl.at/xyHKo";
    public string AvatarUrl { get; private set; } = DEFAULT_AVATAR;
    public ProfileName ProfileName { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Description { get; private set; }

    private Profile() { }

    public static Profile Create(ProfileId id, ProfileName profileName, string? avatarUrl,
        string? firstName, string? lastName, string? description)
    {
        return new Profile
        {
            Id = id,
            ProfileName = profileName,
            AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? DEFAULT_AVATAR : avatarUrl,
            FirstName = firstName,
            LastName = lastName,
            Description = description
        };
    }
    
    public void Update(string? firstName, string? lastName, string? description)
    {
        FirstName = string.IsNullOrWhiteSpace(firstName) ? string.Empty : firstName;
        LastName = string.IsNullOrWhiteSpace(lastName) ? string.Empty : lastName;
        Description = string.IsNullOrWhiteSpace(description) ? string.Empty : description;
    }

    public void UpdateProfileName(ProfileName newProfileName) =>
        ProfileName = newProfileName;
    
    public void UpdateAvatar(string avatarUrl) =>
        AvatarUrl = avatarUrl;
}