namespace Tripex.Core.Domain.Interfaces.Security
{
    public interface IPasswordHasher
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}
