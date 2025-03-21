using Auth.API.Entities;

namespace Auth.API.Services.Interfaces
{
    public interface ITokenService
    {
        Guid GetUserIdByToken();
        void SetTokenWithId(Guid id, string name, int expiresHours);
        public TokenModel GenerateTokens(Guid userId);
        public string GenerateRefreshToken();
    }
}
