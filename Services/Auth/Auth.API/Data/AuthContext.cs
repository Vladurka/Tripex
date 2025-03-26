using Auth.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Data;

public class AuthContext(DbContextOptions<AuthContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
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
        
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Type).IsRequired();
            entity.Property(x => x.Payload).IsRequired();
            entity.Property(x => x.IsPublished).HasDefaultValue(false);
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}