using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Models;

namespace Profiles.Infrastructure.Data;

public class ProfilesContext (DbContextOptions<ProfilesContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles => Set<Profile>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}