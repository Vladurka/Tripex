using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Posts.Domain.Models;
using Posts.Domain.ValueObjects;

namespace Posts.Infrastructure.Configurations;

public class PostCountConfiguration : IEntityTypeConfiguration<PostsCount>
{
    public void Configure(EntityTypeBuilder<PostsCount> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(u => u.ProfileId)
            .HasConversion(name => name.Value, value => ProfileId.Of(value))
            .HasColumnName("ProfileId")
            .ValueGeneratedNever()
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(u => u.ProfileId)
            .IsUnique();

        builder.Property(x => x.Count)
            .IsRequired();
    }
}