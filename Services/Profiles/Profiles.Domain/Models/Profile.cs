using Profiles.Domain.Abstractions;
using Profiles.Domain.ValueObjects;

namespace Profiles.Domain.Models;

public class Profile : Entity<Guid>
{
    private const string DEFAULT_AVATAR = "https://shorturl.at/xyHKo";
    public string AvatarUrl { get; set; } = DEFAULT_AVATAR;
    public UserName UserName { get; private set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Description { get; set; }
    
    public static Profile Create(Guid id, UserName userName)
    {
        var profile = new Profile
        {
            Id = id,
            UserName = userName,
        };

        return profile;
    }
    private Profile() { }
}