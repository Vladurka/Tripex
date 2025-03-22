namespace Profiles.Domain.ValueObjects;

public class UserName
{
    public string Value { get; }
    private UserName(string value) => Value = value;
    public static UserName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        return new UserName(value);
    }
}