using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Controllers
{
    public class AuthController(IUsersService service, IOptions<JwtOptions> jwtOptions, 
        ICensorService censorService, ICookiesService cookiesService) : BaseApiController
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(UserRegister userRegister)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n",
                ModelState.Values.SelectMany(value => value.Errors)
                    .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            var isBad = await censorService.CheckTextAsync(userRegister.UserName);

            if (isBad != "No")
                return BadRequest("User name is not available");

            var user = new User(userRegister.UserName, userRegister.Email, userRegister.Pass);

            return CheckResponse(await service.RegisterAsync(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User(userLogin.Email, userLogin.Pass);
            var result = await service.LoginAsync(user);

            return CheckResponse(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            cookiesService.DeleteCookie(_jwtOptions.TokenName);
            return Unauthorized();
        }
    }
}
