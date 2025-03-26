using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Domain.Models;
using Profiles.Domain.ValueObjects;

namespace Profiles.Infrastructure.Data.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => ProfileId.Of(value))
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .IsRequired();
        
        builder.Property(u => u.ProfileName)
            .HasConversion(name => name.Value, value => ProfileName.Of(value))
            .HasColumnName("ProfileName")
            .ValueGeneratedNever()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.AvatarUrl)
            .IsRequired()
            .HasMaxLength(500);
    
        builder.Property(u => u.Description)
            .HasMaxLength(500);
        
        builder.Property(u => u.FirstName)
            .HasMaxLength(50);
        
        builder.Property(u => u.LastName)
            .HasMaxLength(50);
    }
}
