using Microsoft.AspNetCore.Http;

namespace Posts.Application.Posts.Commands.CreatePost;

public record CreatePostCommand(Guid ProfileId , IFormFile Photo, string? Description) : ICommand<CreatePostResult>;
public record CreatePostResult(Guid Id);

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.Photo).NotNull();
    }
}