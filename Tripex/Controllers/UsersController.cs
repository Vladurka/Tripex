using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Controllers
{
    [Authorize]
    public class UsersController(IUsersService service, ICrudRepository<User> crudRepo,
        IUsersRepository repo, ITokenService tokenService, IS3FileService s3FileService,
        ICookiesService cookiesService, IOptions<JwtOptions> jwtOptions, ICensorService censorService) : BaseApiController
    {

        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User(userLogin.Email, userLogin.Pass);
            var result = await service.LoginAsync(user);

            return CheckResponse(result);
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            cookiesService.DeleteCookie(_jwtOptions.TokenName);
            return Unauthorized();
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
        public async Task<ActionResult<UserGet>> GetMyProfile()
        {
            var user = await GetMyUserAsync();

            return Ok(new UserGet(user));
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<IEnumerable<UserGetMin>>> GetUsersByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest("User name cannot be empty");

            var users = await service.SearchUsersByNameAsync(userName);

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
        public async Task<ActionResult<User>> UpdateAvatar([FromForm] IFormFile file) 
        {
            var user = await GetMyUserAsync();

            var id = tokenService.GetUserIdByToken();

            if(user.AvatarUrl != "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS5CQxdTYvVk0IxK9JjTg3YaEPXKfuPfCK3mg&s")
                await s3FileService.DeleteFileAsync(id.ToString());

            user.AvatarUrl = await s3FileService.UploadFileAsync(file, id.ToString());
            user.AvatarUpdated = DateTime.UtcNow;
            user.Updated = DateTime.UtcNow;

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
            user.Updated = DateTime.UtcNow;

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
            user.Updated = DateTime.UtcNow;

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
