using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Core.Services.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyPassword(string hashedPassword, string providedPassword) =>
            BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}
