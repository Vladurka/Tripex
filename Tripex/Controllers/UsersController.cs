using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Controllers
{
    public class UsersController(IUsersService service, ICrudRepository<User> crudRepo,
        IUsersRepository repo, ITokenService tokenService) : BaseApiController
    {

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

        [HttpGet("profiles")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersProfile()
        {
            var users = await service.GetUsersAsync();
            var usersGet = users.Select(user => new UserGet(user))
                .ToList();

            return Ok(usersGet);
        }

        [HttpGet("profile/my")]
        public async Task<ActionResult<IEnumerable<User>>> GetMyProfile()
        {
            var user = await GetMyUserAsync();

            return Ok(new UserGet(user));
        }

        [HttpGet("{userName}/{pageIndex:int}")]
        public async Task<ActionResult<IEnumerable<UserGetMin>>> GetUsersByName(string userName, int pageIndex)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest("User name cannot be empty");

            var users = await service.SearchUsersByNameAsync(userName, pageIndex);
            var usersGet = users.Select(user => new UserGetMin(user))
                .ToList();

            return Ok(usersGet);
        }

        [HttpGet("profile/{id:guid}")]
        public async Task<ActionResult<UserGet>> GetUsersProfileByName(Guid id)
        {
            var user = await service.GetUserByIdAsync(id);

            return Ok(new UserGet(user));
        }

        [HttpPut("avatar")]
        public async Task<ActionResult<User>> UpdateAvatar([FromBody] string avatarUrl) 
        {
            if (!Uri.TryCreate(avatarUrl, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                return BadRequest("Invalid URL format for avatar.");

            var user = await GetMyUserAsync();

            user.AvatarUrl = avatarUrl;
            await crudRepo.UpdateAsync(user);
            return Ok(new UserGet(user));
        }

        [HttpPut("description/{description}")]
        public async Task<ActionResult<User>> UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return BadRequest("Description can't be empty");

            var user = await GetMyUserAsync();

            user.Description = description;
            await crudRepo.UpdateAsync(user);
            return Ok(new UserGet(user));
        }

        [HttpPut("userName/{userName}")]
        public async Task<ActionResult<User>> UpdateUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest("User name can't be empty");

            if(await repo.UsernameExistsAsync(userName))
                return BadRequest("This user name already exists");

            var user = await GetMyUserAsync();

            user.UserName = userName;
            await crudRepo.UpdateAsync(user);

            return Ok(new UserGet(user));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<IEnumerable<User>>> DeleteUserById(Guid id) =>
            CheckResponse(await crudRepo.RemoveAsync(id));

        private async Task<User> GetMyUserAsync()
        {
            var id = tokenService.GetUserIdByToken();
            var user = await service.GetUserByIdAsync(id);
            return user;
        }
    }
}
