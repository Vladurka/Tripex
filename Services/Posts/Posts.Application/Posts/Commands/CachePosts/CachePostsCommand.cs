namespace Posts.Application.Posts.Commands.CachePosts;

public record CachePostsCommand(Guid ProfileId) : ICommand;

public class CachePostsCommandValidator : AbstractValidator<CachePostsCommand>
{
    public CachePostsCommandValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}