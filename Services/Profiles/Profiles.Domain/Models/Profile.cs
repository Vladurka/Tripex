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
    public int ViewCount { get; private set; }
    public DateTime ViewCountResetAt { get; private set; } = DateTime.UtcNow;
    public bool IsCached { get; private set; }
    
    public const int VIEW_COUNT_UPDATE_TIME = 7;
    public const int COUNT_TRIGGER = 2;

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
            Description = description,
            ViewCount = 1
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
        AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? DEFAULT_AVATAR : avatarUrl;

    #region Caching
    public void UpdateViewCount()
    {
        ResetViewCountIfOutdated();
        ViewCount++;
        LastModified = DateTime.UtcNow;
    }

    private void ResetViewCountIfOutdated()
    {
        if ((DateTime.UtcNow - ViewCountResetAt).TotalDays > VIEW_COUNT_UPDATE_TIME)
            ResetViewCount();
    }

    private void ResetViewCount()
    {
        ViewCount = 0;
        ViewCountResetAt = DateTime.UtcNow;
    }

    public void SetIsCached(bool value) =>
        IsCached = value;

    public bool ShouldBeCached()
    {
        UpdateViewCount();

        if (ViewCount >= COUNT_TRIGGER)
        {
            ResetViewCount();
            IsCached = true;
            return true;
        }
        
        return false;
    }
    #endregion
}