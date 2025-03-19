using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Data;

namespace Profiles.Infrastructure.Data;

public class ProfilesRepository(ProfilesContext context) : IProfilesRepository
{
    public async Task AddAsync(Profile entity)
    {
        await context.Profiles.AddAsync(entity);
        await SaveChangesAsync();
    }

    public IQueryable<Profile> GetQueryable() =>
        context.Profiles;

    public async Task<Profile?> GetByIdAsync(Guid id) =>
        await context.Profiles.FirstOrDefaultAsync(x => x.Id.Value == id);

    public async Task UpdateAsync(Profile entity)
    {
        if (await GetByIdAsync(entity.Id.Value) == null)
            throw new NotFoundException(entity, entity.Id);

        context.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync();
    }
    public async Task RemoveAsync(Guid id)
    {
        var profile = await GetByIdAsync(id);

        if(profile == null)
            throw new NotFoundException("Profile", id);

        context.Profiles.Remove(profile);
        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        if (await context.SaveChangesAsync() <= 0)
            throw new InvalidOperationException("Could not save changes");
    }
}