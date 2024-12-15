using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.User;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class UsersController(IUsersService service, ICrudRepository<User> repo) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(UserRegister user)
        {
            return CheckResponse(await service.RegisterAsync(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(UserLogin user)
        {
            return CheckResponse(await service.LoginAsync(user));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await service.GetUsersAsync());
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            return Ok(await service.GetUserInfoByIdAsync(id));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<User>> DeleteUserById(Guid id)
        {
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
