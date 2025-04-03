using System.Reflection;
using Posts.Application.Data;
using Posts.Domain.Models;

namespace Posts.Infrastructure.Data;

public class PostCountContext(DbContextOptions<PostCountContext> options) 
    : DbContext(options), IPostCountContext
{
    public DbSet<PostsCount> PostCount { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}