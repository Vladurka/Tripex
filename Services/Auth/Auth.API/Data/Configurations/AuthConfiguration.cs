using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.API.Data.Configurations;

public class AuthConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
            
        builder.HasIndex(u => u.Id)
            .IsUnique();

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.HasIndex(u => u.UserName)
            .IsUnique();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(u => u.RefreshToken)
            .IsRequired()
            .HasMaxLength(255);
    }
}