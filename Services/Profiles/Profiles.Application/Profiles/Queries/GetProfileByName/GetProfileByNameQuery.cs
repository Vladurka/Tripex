namespace Profiles.Application.Profiles.Queries.GetProfileByName;

public record GetProfileByUserNameQuery(string UserName) : IQuery<GetProfileResult>;

public class GetProfileByUserNameQueryValidator : AbstractValidator<GetProfileByUserNameQuery>
{
    public GetProfileByUserNameQueryValidator() =>
        RuleFor(x => x.UserName).NotEmpty();
}