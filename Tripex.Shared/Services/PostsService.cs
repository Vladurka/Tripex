using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Core.Services
{
    public class PostsService(ICrudRepository<Post> repo, ITokenService tokenService) : IPostsService
    {
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

        public async Task<ResponseOptions> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if(post.UserId != tokenService.GetUserIdByToken())
                return ResponseOptions.BadRequest;

            await repo.RemoveAsync(postId);      
            return ResponseOptions.Ok;
        }

        private async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await repo.GetQueryable<Post>()
               .Include(p => p.User)
               .Include(p => p.Comments)
                   .ThenInclude(c => c.User)
               .Include(p => p.Likes)
                   .ThenInclude(l => l.User)
               .SingleOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                throw new KeyNotFoundException($"Post with id {postId} not found");

            return post;
        }
    }
}
