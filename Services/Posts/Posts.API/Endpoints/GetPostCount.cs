using Posts.Application.Posts.Queries.GetPostCount;

namespace Posts.API.Endpoints;

public class GetPostCount : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/posts/postcount/{profileId:guid}", async (Guid profileId, ISender sender) =>
            {
                var result = await sender.Send(new GetPostCountQuery(profileId));
                Console.WriteLine(profileId);
                return Results.Ok(result);
            })
            .WithName("GetPostCount")
            .Produces<GetPostCountResult>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get post count by id")
            .WithDescription("Get post count by id");
    }
}