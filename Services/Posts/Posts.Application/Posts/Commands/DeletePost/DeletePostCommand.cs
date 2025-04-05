namespace Posts.Application.Posts.Commands.DeletePost;

public record DeletePostCommand(Guid ProfileId, Guid PostId) : ICommand<DeletePostResult>;
public record DeletePostResult(bool IsSucceed);

public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    DeletePostCommandValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.PostId).NotEmpty();
    }
}