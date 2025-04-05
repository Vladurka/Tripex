using Microsoft.AspNetCore.Mvc;
using Posts.Application.Posts.Commands.DeletePost;

namespace Posts.API.Endpoints;

public class DeletePost : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/posts", async ([FromBody] DeletePostCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .DisableAntiforgery()
        .WithName("DeletePost")
        .Produces<DeletePostResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete post")
        .WithDescription("Delete post");
    }
}