using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Likes;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class UsersController(IUsersService service, ICrudRepository<User> repo) : BaseApiController
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

            return CheckResponse(await service.LoginAsync(user));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await service.GetUsersAsync();
            var usersGet = new List<UserGet>(users.Count());

            foreach (var user in users)
                usersGet.Add(new UserGet(user));

            return Ok(usersGet);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await service.GetUserInfoByIdAsync(id);
            var userGet = new UserGet(user);
            return Ok(userGet);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<User>> DeleteUserById(Guid id)
        {
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
