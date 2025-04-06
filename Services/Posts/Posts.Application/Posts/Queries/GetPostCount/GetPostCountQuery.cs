namespace Posts.Application.Posts.Queries.GetPostCount;

public record GetPostCountQuery(Guid ProfileId) : IQuery<GetPostCountResult>;
public record GetPostCountResult(int PostCount);

public class GetPostCountQueryValidator : AbstractValidator<GetPostCountQuery>
{
    public GetPostCountQueryValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}