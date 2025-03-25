using Profiles.Domain.Abstractions;
using Profiles.Domain.ValueObjects;

namespace Profiles.Domain.Models;
public class Profile : Entity<ProfileId>
{
    private const string DEFAULT_AVATAR = "https://shorturl.at/xyHKo";
    
    public string AvatarUrl { get; private set; } = DEFAULT_AVATAR;
    public UserName UserName { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Description { get; private set; }

    private Profile() { }

    public static Profile Create(ProfileId id, UserName userName)
    {
        return new Profile
        {
            Id = id,
            UserName = userName,
        };
    }

    public static Profile Create(ProfileId id, UserName userName, string avatarUrl,
        string? firstName, string? lastName, string? description)
    {
        return new Profile
        {
            Id = id,
            UserName = userName,
            AvatarUrl = avatarUrl,
            FirstName = firstName,
            LastName = lastName,
            Description = description
        };
    }
    
    public void Update(string? avatarUrl, string? firstName, string? lastName, string? description)
    {
        AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? AvatarUrl : avatarUrl;
        FirstName = string.IsNullOrWhiteSpace(firstName) ? FirstName : firstName;
        LastName = string.IsNullOrWhiteSpace(lastName) ? LastName : lastName;
        Description = string.IsNullOrWhiteSpace(description) ? Description : description;
    }

    public void UpdateUserName(UserName newUserName) =>
        UserName = newUserName;
}