namespace Profiles.Application.Profiles.Queries.GetBaseInfo;

public record GetBaseInfoQuery(Guid ProfileId) : IQuery<GetBaseInfoResult>;
public record GetBaseInfoResult(Guid ProfileId, string ProfileName, string AvatarUrl);

public class GetBaseInfoQueryValidator : AbstractValidator<GetBaseInfoQuery>
{
    public GetBaseInfoQueryValidator() =>
        RuleFor(x => x.ProfileId).NotEmpty();
}