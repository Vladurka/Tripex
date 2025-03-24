using Profiles.Domain.Abstractions;
using Profiles.Domain.ValueObjects;

namespace Profiles.Domain.Models;

public class Profile : Entity<ProfileId>
{
    private const string DEFAULT_AVATAR = "https://shorturl.at/xyHKo";
    public string AvatarUrl { get; set; } = DEFAULT_AVATAR;
    public UserName UserName { get; private set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Description { get; set; }
    
    public static Profile Create(ProfileId id, UserName userName)
    {
        var profile = new Profile
        {
            Id = id,
            UserName = userName,
        };

        return profile;
    }
    
    public static Profile Create(ProfileId id, UserName userName, string avatarUrl,
    string? firstName, string? lastName, string? description)
    {
        var profile = new Profile
        {
            Id = id,
            UserName = userName,
            AvatarUrl = avatarUrl,
            FirstName = firstName,
            LastName = lastName,
            Description = description
        };

        return profile;
    }
    private Profile() { }
}