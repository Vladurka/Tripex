using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tripex.Core.Domain.Interfaces.Services.Security;
using Microsoft.AspNetCore.Http;
using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Services.Security
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICookiesService _cookiesManager;

        private readonly JwtOptions _options;
        public TokenService(IOptions<JwtOptions> options, IHttpContextAccessor httpContextAccessor,
            ICookiesService cookiesManager) 
        {
            _options = options.Value;
            _httpContextAccessor = httpContextAccessor;
            _cookiesManager = cookiesManager;
        }

        public Guid GetUserIdByToken()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                throw new Exception("User is not authenticated");

            var idString = user.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (idString == null)
                throw new Exception("Id not found in token");

            Guid id = Guid.Parse(idString);

            return id;
        }

        public void SetTokenWithId(Guid id, string name, int expiresHours)
        {
            string token = GenerateToken(id);

            if (expiresHours <= 0)
                throw new ArgumentNullException(nameof(expiresHours));

            if (!_cookiesManager.CookieExists(name))
                _cookiesManager.AddCookie(name, token, expiresHours);

            else
                _cookiesManager.UpdateCookie(name, token, expiresHours);
        }

        private string GenerateToken(Guid id)
        {
            Claim[] claims = [new("userId", id.ToString())];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecurityKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return tokenValue;
        }
    }
}
