using BuildingBlocks.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Profiles.Application.Data;
using Profiles.Domain.ValueObjects;

namespace Profiles.Infrastructure.Data;

public class ProfilesRepository(ProfilesContext context) : IProfilesRepository
{
    public async Task CreateProfileAsync(Profile profile, CancellationToken cancellationToken)
    {
        await context.Profiles.AddAsync(profile, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public IQueryable<Profile> GetQueryable() =>
        context.Profiles.AsQueryable();

    public async Task<Profile?> GetProfileByIdAsync(Guid id, CancellationToken cancellationToken, bool asNoTracking = true)
    {
        IQueryable<Profile> query = context.Profiles;

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(p => p.Id == ProfileId.Of(id), cancellationToken);
    }

    public async Task<bool> ProfileNameExistsAsync(string userName, CancellationToken cancellationToken) =>
        await context.Profiles.AnyAsync(x => x.ProfileName == ProfileName.Of(userName), cancellationToken);

    public async Task RemoveProfileAsync(Guid id, CancellationToken cancellationToken)
    {
        var profile = await GetProfileByIdAsync(id, cancellationToken);

        if(profile == null)
            throw new NotFoundException("Profile", id);

        context.Profiles.Remove(profile);
        await SaveChangesAsync(cancellationToken);
    }
    
    public async Task RemoveProfileAsync(Profile profile, CancellationToken cancellationToken)
    {
        context.Profiles.Remove(profile);
        await SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken) =>
        await context.Database.BeginTransactionAsync(cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken, bool shouldUpdate = true)
    {
        var result = await context.SaveChangesAsync(cancellationToken);
        
        if(result <= 0 && shouldUpdate)
            throw new InvalidOperationException("Could not save changes");
    }
}