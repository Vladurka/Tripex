using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Domain.Models;

namespace Profiles.Infrastructure.Data.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.AvatarUrl).IsRequired().HasMaxLength(500);
        builder.Property(u => u.Description).HasMaxLength(500);

        builder.OwnsOne(u => u.UserName, username =>
        {
            username.Property(u => u.Value)
                .IsRequired()
                .HasMaxLength(100);
        });
    }
}
