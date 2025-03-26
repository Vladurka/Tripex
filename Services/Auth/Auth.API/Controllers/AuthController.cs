namespace Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IOptions<JwtOptions> jwtOptions, ICookiesService cookiesService, 
    IUserService userService) : ControllerBase
{
    private JwtOptions _jwtOptions => jwtOptions.Value;

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

        var response = await userService.RegisterAsync(dto); 
        return Ok(new { RefreshToken = response.Token.RefreshToken, Id = response.Id }); 
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

        var tokens = await userService.LoginAsync(dto);
        return Ok(new { RefreshToken = tokens.RefreshToken }); 
    }

    [HttpPost("refresh/{refreshToken}")]
    public async Task<ActionResult> Refresh(string refreshToken)
    {
        var tokens = await userService.RefreshAsync(refreshToken);
        return Ok(new { RefreshToken = tokens.RefreshToken }); 
    }

    [Authorize]
    [HttpPost("logout")]
    public ActionResult Logout()
    {
        cookiesService.DeleteCookie(_jwtOptions.TokenName);
        return Unauthorized(); 
    }
}