namespace Tripex.Core.Domain.Entities
{
    public class JwtOptions
    {
        public string TokenName { get; set; } = string.Empty;
        public string SecurityKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}