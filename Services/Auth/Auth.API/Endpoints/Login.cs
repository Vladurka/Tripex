using Auth.API.Auth.Commands.Login;

namespace Auth.API.Endpoints;

public class Login : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/login", async (LoginCommand command, ISender sender) =>{
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        }})
        .WithName("Login")
        .Produces<LoginResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Login")
        .WithDescription("Login");
    }
}