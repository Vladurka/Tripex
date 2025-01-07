using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Interfaces.Services.Security;
using Tripex.Core.Enums;

namespace Tripex.Core.Services
{
    public class PostsService(ICrudRepository<Post> repo, ICrudRepository<User> usersCrudRepo, 
        ITokenService tokenService, IUsersService usersService, IS3FileService s3FileService) : IPostsService
    {
        public async Task AddPostAsync(Post post) 
        {
            await using var transaction = await repo.BeginTransactionAsync();

            await repo.AddAsync(post);
            var user = await usersService.GetUserByIdAsync(post.UserId);

            user.PostsCount++;
            await usersCrudRepo.UpdateAsync(user);

            await transaction.CommitAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId, int pageIndex, int pageSize = 20)
        {
            var posts = await repo.GetQueryable<Post>()
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var post in posts)
            {
                await post.UpdateContentUrlIfNeededAsync(s3FileService, repo);
                await post.User.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo);
                await post.UpdateViewedCountAsync(repo);
            }

            return posts;
        }

        public async Task<ResponseOptions> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if(post.UserId != tokenService.GetUserIdByToken())
                return ResponseOptions.BadRequest;

            await using var transaction = await repo.BeginTransactionAsync();
            var user = await usersService.GetUserByIdAsync(post.UserId);
            user.PostsCount--;

            await s3FileService.DeleteFileAsync(postId.ToString());
            await repo.RemoveAsync(postId);

            await usersCrudRepo.UpdateAsync(user);
            await transaction.CommitAsync();

            return ResponseOptions.Ok;
        }

        private async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await repo.GetQueryable<Post>()
               .Include(p => p.User)
               .SingleOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                throw new KeyNotFoundException($"Post with id {postId} not found");

            await post.UpdateContentUrlIfNeededAsync(s3FileService, repo);
            await post.User.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo);
            await post.UpdateViewedCountAsync(repo);

            return post;
        }
    }
}
