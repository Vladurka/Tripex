using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Profiles.Infrastructure.Data.Configurations;

namespace Profiles.Infrastructure.Data;

public class ProfilesContext (DbContextOptions<ProfilesContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}