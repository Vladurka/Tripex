using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Auth;

public class JwtHelper(IHttpContextAccessor httpContextAccessor) : IJwtHelper
{
    public Guid GetUserIdByToken()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
            throw new UnauthorizedAccessException("User is not authenticated");

        var idClaim = user.FindFirst("userId");

        if (idClaim is null || string.IsNullOrWhiteSpace(idClaim.Value))
            throw new NotFoundException("userId not found in token");

        return Guid.Parse(idClaim.Value);
    }
}