using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Data;

public class AuthContext(DbContextOptions<AuthContext> options) : DbContext(options), IOutboxContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.ApplyConfigurationsFromAssembly(typeof(OutboxMessageConfiguration).Assembly);
    }
}