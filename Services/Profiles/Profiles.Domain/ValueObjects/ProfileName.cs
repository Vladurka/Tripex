namespace Profiles.Domain.ValueObjects;

public class ProfileName
{
    public string Value { get; }
    private ProfileName(string value) => Value = value;
    public static ProfileName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return new ProfileName(value);
    }
}