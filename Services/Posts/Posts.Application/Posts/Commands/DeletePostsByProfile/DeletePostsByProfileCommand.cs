namespace Posts.Application.Posts.Commands.DeletePostsByProfile;

public record DeletePostsByProfileCommand(Guid ProfileId) : ICommand;

public class DeletePostsByProfileCommandValidator : AbstractValidator<DeletePostsByProfileCommand>
{
    public DeletePostsByProfileCommandValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}