using System.Reflection;
using BuildingBlocks.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Profiles.Infrastructure.Data;

public class ProfilesContext (DbContextOptions<ProfilesContext> options) : DbContext(options), IOutboxContext
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.ApplyConfigurationsFromAssembly(typeof(OutboxMessageConfiguration).Assembly);
        base.OnModelCreating(builder);
    }
}