namespace Profiles.Application.Profiles.Queries.SearchProfilesByName;

public record SearchProfilesByNameQuery(string ProfileName) : IQuery<SearchProfilesByNameResult>;

public record SearchProfilesByNameResult(IEnumerable<GetProfileResult> Profiles);

public class GetProfileByUserNameQueryValidator : AbstractValidator<SearchProfilesByNameQuery>
{
    public GetProfileByUserNameQueryValidator() =>
        RuleFor(x => x.ProfileName).NotEmpty();
}