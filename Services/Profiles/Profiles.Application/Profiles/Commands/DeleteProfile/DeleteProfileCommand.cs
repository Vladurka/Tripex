namespace Profiles.Application.Profiles.Commands.DeleteProfile;

public record DeleteProfileCommand(Guid ProfileId) : ICommand<DeleteProfileResult>;
public record DeleteProfileResult(bool Succeed) ;

public class DeleteProfileCommandValidator : AbstractValidator<DeleteProfileCommand>
{
    public DeleteProfileCommandValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}
