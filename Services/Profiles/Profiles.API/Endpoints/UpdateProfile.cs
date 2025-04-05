using Profiles.Application.Profiles.Commands.UpdateProfile;
using Profiles.Application.Profiles.Queries;

namespace Profiles.API.Endpoints;

public class UpdateProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/profiles", async (UpdateProfileCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("UpdateProfile")
        .Produces<GetProfileResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update profile")
        .WithDescription("Update profile");
    }
}