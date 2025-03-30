using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Profiles.Commands.UpdateAvatar;

namespace Profiles.API.Endpoints;

public class UpdateAvatar : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/profiles/avatar", async ([FromForm] UpdateAvatarCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return Results.Ok(result);
        })
        .DisableAntiforgery() 
        .WithName("UpdateAvatar")
        .Produces<UpdateAvatarResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update avatar")
        .WithDescription("Update avatar");
    }
}