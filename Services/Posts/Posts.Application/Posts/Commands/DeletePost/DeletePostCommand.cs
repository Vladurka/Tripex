namespace Posts.Application.Posts.Commands.DeletePost;

public record DeletePostCommand : ICommand<DeletePostResult>
{
    public Guid ProfileId { get; set; }
    public Guid PostId { get; init; }
}
public record DeletePostResult(bool IsSucceed);

public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator() =>
        RuleFor(x => x.PostId).NotEmpty();
}