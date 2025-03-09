namespace Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IOptions<JwtOptions> jwtOptions, ICookiesService cookiesService,
    IUserService service) : ControllerBase
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            string errors = string.Join("\n",
                ModelState.Values.SelectMany(value => value.Errors)
                    .Select(err => err.ErrorMessage));

            return BadRequest(errors);
        }

        await service.RegisterAsync(dto);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUser(LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            string errors = string.Join("\n",
                ModelState.Values.SelectMany(value => value.Errors)
                    .Select(err => err.ErrorMessage));

            return BadRequest(errors);
        }
        
        await service.LoginAsync(dto);

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public ActionResult Logout()
    {
        cookiesService.DeleteCookie(_jwtOptions.TokenName);
        return Unauthorized();
    }
}