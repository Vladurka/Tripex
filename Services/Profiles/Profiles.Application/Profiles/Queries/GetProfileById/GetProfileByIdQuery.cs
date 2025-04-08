namespace Profiles.Application.Profiles.Queries.GetProfileById;

public record GetProfileByIdQuery(Guid ProfileId) : IQuery<GetProfileResult>;

public class GetProfileByIdQueryValidator : AbstractValidator<GetProfileByIdQuery>
{
    public GetProfileByIdQueryValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}
    