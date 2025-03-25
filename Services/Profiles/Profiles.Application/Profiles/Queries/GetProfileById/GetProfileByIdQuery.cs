namespace Profiles.Application.Profiles.Queries.GetProfileById;

public record GetProfileByIdQuery(Guid UserId) : IQuery<GetProfileResult>;

public class GetProfileByIdQueryValidator : AbstractValidator<GetProfileByIdQuery>
{
    public GetProfileByIdQueryValidator() =>
        RuleFor(x => x.UserId).NotEmpty();
}
    