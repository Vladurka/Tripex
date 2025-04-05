using Microsoft.AspNetCore.Http;

namespace Posts.Application.Posts.Commands.CreatePost;

public record CreatePostCommand : ICommand<CreatePostResult>
{
    public Guid ProfileId { get; set; }
    public IFormFile Photo { get; init; }
    public string? Description{ get; init; }
}
public record CreatePostResult(Guid Id);

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Photo).NotNull();
    }
}