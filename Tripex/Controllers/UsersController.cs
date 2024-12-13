using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.User;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class UsersController(IUsersService service) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserRegister user)
        {
            return CheckResponse(await service.RegisterAsync(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserLogin user)
        {
            return CheckResponse(await service.LoginAsync(user));
        }
    }
}
