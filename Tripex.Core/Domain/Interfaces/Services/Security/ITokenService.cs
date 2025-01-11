namespace Tripex.Core.Domain.Interfaces.Services.Security
{
    public interface ITokenService
    {
        public Guid GetUserIdByToken();
        public void SetTokenWithId(Guid id, string name, int expiresHours);
    }
}
