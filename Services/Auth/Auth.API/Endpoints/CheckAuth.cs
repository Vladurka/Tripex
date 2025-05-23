using BuildingBlocks.Auth;

namespace Auth.API.Endpoints;

public class CheckAuth : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/auth/check", (IJwtHelper helper) => 
                Results.Ok(helper.GetUserIdByToken()))
        .RequireAuthorization()
        .WithName("CheckAuth")
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("CheckAuth")
        .WithDescription("CheckAuth");
    }
}