using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Interfaces.Services.Security;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tripex.Core.Services
{
    public class PostsService(ICrudRepository<Post> repo, ICrudRepository<User> usersCrudRepo, 
        ITokenService tokenService, IUsersService usersService) : IPostsService
    {
        public async Task AddPostAsync(Post post) 
        {
            await repo.AddAsync(post);
            var user = await usersService.GetUserByIdAsync(post.UserId);

            user.PostsCount++;
            await usersCrudRepo.UpdateAsync(user);
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId, int pageIndex, int pageSize = 20)
        {
            var posts = await repo.GetQueryable<Post>()
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .AsNoTracking()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return posts;
        }

        public async Task<ResponseOptions> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if(post.UserId != tokenService.GetUserIdByToken())
                return ResponseOptions.BadRequest;

            var user = await usersService.GetUserByIdAsync(post.UserId);
            user.PostsCount--;

            await repo.RemoveAsync(postId);
            await usersCrudRepo.UpdateAsync(user);
            return ResponseOptions.Ok;
        }

        private async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await repo.GetQueryable<Post>()
               .Include(p => p.User)
               .AsNoTracking()
               .SingleOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                throw new KeyNotFoundException($"Post with id {postId} not found");

            return post;
        }
    }
}
