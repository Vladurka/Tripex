using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Controllers
{
    public class UsersController(IUsersService service, ICrudRepository<User> repo,
        ITokenService tokenService) : BaseApiController
    {   

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(UserRegister userRegister)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await service.GetUsersAsync();
            var usersGet = users.Select(user => new UserGet(user))
                .ToList();

            return Ok(usersGet);
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<User>>> GetMyProfile()
        {
            var id = tokenService.GetUserIdByToken();
            var user = await service.GetUserByIdAsync(id);

            return Ok(new UserGet(user));
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<IEnumerable<UserGet>>> GetUsersByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest("User name cannot be empty");

            var users = await service.GetUsersByNameAsync(userName);
            var usersGet = users.Select(user => new UserGet(user))
                .ToList();

            return Ok(usersGet);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<IEnumerable<User>>> DeleteUserById(Guid id)
        {
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
