using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Data;
using Profiles.Domain.ValueObjects;

namespace Profiles.Infrastructure.Data;

public class ProfilesRepository(ProfilesContext context) : IProfilesRepository
{
    public async Task AddAsync(Profile entity)
    {
        await context.Profiles.AddAsync(entity);
        await SaveChangesAsync();
    }

    public IQueryable<Profile> GetQueryable() =>
        context.Profiles.AsQueryable();

    public async Task<Profile?> GetByIdAsync(Guid id, bool asNoTracking = false)
    {
        IQueryable<Profile> query = context.Profiles;

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(p => p.Id == ProfileId.Of(id));
    }
    public async Task<bool> ProfileNameExistsAsync(string userName) =>
        await context.Profiles.AnyAsync(x => x.ProfileName == ProfileName.Of(userName));

    public async Task UpdateAsync(Profile entity)
    {
        var existingProfile = await GetByIdAsync(entity.Id.Value);
        
        if (existingProfile == null)
            throw new NotFoundException(entity, entity.Id);

        existingProfile.Update(entity.AvatarUrl, entity.FirstName, entity.LastName, entity.Description);
    
        if (existingProfile.ProfileName != entity.ProfileName)
            existingProfile.UpdateUserName(entity.ProfileName);

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