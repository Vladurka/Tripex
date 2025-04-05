using Auth.API.Auth.Commands.Register;

namespace Auth.API.Endpoints;

public class Register : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/register", async (RegisterCommand command, ISender sender) =>{
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            }})
            .WithName("Register")
            .Produces<RegisterResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Register")
            .WithDescription("Register");
    }
}