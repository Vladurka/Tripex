namespace Auth.API.Endpoints;


public class Logout
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/logout", 
                (IOptions<JwtOptions> jwtOptions, ICookiesService cookiesService) =>
            {
                cookiesService.DeleteCookie(jwtOptions.Value.TokenName);
                return Results.NoContent(); 
            })
            .RequireAuthorization()
            .WithName("Logout")
            .WithSummary("Logout")
            .WithDescription("Logs the user out and deletes the auth token cookie.");
    }
}