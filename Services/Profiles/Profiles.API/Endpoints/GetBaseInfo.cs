using Profiles.Application.Profiles.Queries.GetBaseInfo;

namespace Profiles.API.Endpoints;

public class GetBaseInfo : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/profiles/base/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetBaseInfoQuery(id));
                return Results.Ok(result);
            })
            .WithName("GetBaseInfo")
            .Produces<GetBaseInfoQuery>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get base info")
            .WithDescription("Get base info");
    }
}