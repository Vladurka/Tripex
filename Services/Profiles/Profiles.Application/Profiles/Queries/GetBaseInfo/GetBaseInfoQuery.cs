namespace Profiles.Application.Profiles.Queries.GetBaseInfo;

public record GetBaseInfoQuery(Guid UserId) : IQuery<GetBaseInfoResult>;
public record GetBaseInfoResult(Guid UserId, string ProfileName, string AvatarUrl);

public class GetBaseInfoQueryValidator : AbstractValidator<GetBaseInfoQuery>
{
    public GetBaseInfoQueryValidator() =>
        RuleFor(x => x.UserId).NotEmpty();
}