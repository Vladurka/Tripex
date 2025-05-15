namespace Profiles.Application.Profiles.Commands.DecrementPostCount;

public class DecrementPostCountHandler(IProfilesRepository repo,
    IProfilesRedisRepository redisRepo) : ICommandHandler<DecrementPostCountCommand>
{
    public async Task<Unit> Handle(DecrementPostCountCommand command, CancellationToken cancellationToken)
    {
        var profileId = ProfileId.Of(command.ProfileId);

        var profile = await repo.GetProfileByIdAsync(profileId, cancellationToken, false) ??
            throw new NotFoundException("Profile", command.ProfileId);

        profile.DecrementPostCount();

        List<Task> tasks = new()
        {
            repo.SaveChangesAsync(cancellationToken)
        };

        if (profile.IsCached)
            tasks.Add(redisRepo.UpdateProfileAsync(profile));

        await Task.WhenAll(tasks);
        return Unit.Value;
    }
}
