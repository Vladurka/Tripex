namespace Posts.Application.Posts.Queries.GetPostsByUser;

public record GetPostsByUserQuery(Guid ProfileId) : IQuery<GetPostsByUserResult>;
public record GetPostsByUserResult(IEnumerable<PostDto> Posts);

public class GetPostsByUserQueryValidator : AbstractValidator<GetPostsByUserQuery>
{
    public GetPostsByUserQueryValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}