using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Tripex.Core.Services
{
    public class PostsService(ICrudRepository<Post> repo) : IPostsService
    {
        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await repo.GetQueryable<Post>()
                .Where(p => p.Id == postId)
                .Include(p => p.User)                
                .Include(p => p.Comments)   
                    .ThenInclude(c => c.User)    
                .Include(p => p.Likes)
                    .ThenInclude(l => l.User)
                .SingleOrDefaultAsync();

            if (post == null)
                throw new KeyNotFoundException($"Post with id {postId} not found");

            return post;
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId)
        {
            var posts = await repo.GetQueryable<Post>()
                .Where(p => p.UserId == userId)
                .Include(p => p.User)                    
                .Include(p => p.Comments)                
                    .ThenInclude(c => c.User)           
                .Include(p => p.Likes)               
                    .ThenInclude(l => l.User)
                .ToListAsync();

            return posts;
        }
    }
}
