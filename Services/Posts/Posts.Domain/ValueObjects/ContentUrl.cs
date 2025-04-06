namespace Posts.Domain.ValueObjects;

public class ContentUrl
{
    public string Value { get; }

    private ContentUrl(string value) => Value = value;

    public static ContentUrl Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("ContentUrl cannot be empty.");
        
        value = value.Trim();
        
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            throw new DomainException("ContentUrl must be a valid HTTP or HTTPS URL.");

        return new ContentUrl(value);
    }
}