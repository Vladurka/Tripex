namespace Auth.API.Services.Interfaces
{
    public interface ITokenService
    {
        public Guid GetUserIdByToken();
        public void SetTokenWithId(Guid id, string name, int expiresHours);
    }
}
