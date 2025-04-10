namespace Posts.Application.Posts.Commands.CachePosts;

public record CachePostsCommand(Guid ProfileId) : ICommand<CachePostsResult>;
public record CachePostsResult(bool IsSucceed);

public class CachePostsCommandValidator : AbstractValidator<CachePostsCommand>
{
    public CachePostsCommandValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}