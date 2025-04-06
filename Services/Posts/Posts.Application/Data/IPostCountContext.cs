using Microsoft.EntityFrameworkCore;

namespace Posts.Application.Data;

public interface IPostCountContext
{
    public DbSet<PostsCount> PostCount { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}