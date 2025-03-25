using Mapster;
using Profiles.Application.Profiles.Commands.DeleteProfile;

namespace Profiles.API.Endpoints;

public class DeleteProfile : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/profiles/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProfileCommand(id));
                return Results.Ok(result);
            })
            .WithName("DeleteProfile")
            .Produces<DeleteProfileResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete profile")
            .WithDescription("Delete profile");
    }
}