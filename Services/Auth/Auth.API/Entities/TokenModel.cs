namespace Auth.API.Entities;

public class TokenModel
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; }= string.Empty;
}