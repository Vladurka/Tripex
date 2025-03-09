namespace Tripex.Core.Domain.Entities
{
    public class JwtOptions
    {
        public string SecurityKey { get; set; } = string.Empty;
        public int ExpiresHours { get; set; }
        public string TokenName { get; set; } = string.Empty;
    }
}