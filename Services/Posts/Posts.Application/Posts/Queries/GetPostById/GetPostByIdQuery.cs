namespace Posts.Application.Posts.Queries.GetPostById;

public record GetPostByIdQuery(Guid Id) : IQuery<GetPostByIdResult>;
public record GetPostByIdResult(PostDto Post);

public class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
{
    public GetPostByIdQueryValidator() => 
        RuleFor(x => x.Id).NotEmpty();
}

