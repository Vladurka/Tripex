using Microsoft.EntityFrameworkCore;

namespace Auth.API.Data;

public class AuthContext(DbContextOptions<AuthContext> options) : DbContext(options), IOutboxContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(u => u.RefreshToken)
                .IsRequired()
                .HasMaxLength(255);
        });
        
        builder.ApplyConfigurationsFromAssembly(typeof(OutboxMessageConfiguration).Assembly);
    }
}