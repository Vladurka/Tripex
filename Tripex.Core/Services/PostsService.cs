using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Interfaces.Services.Security;
using Tripex.Core.Enums;

namespace Tripex.Core.Services
{
    public class PostsService(ICrudRepository<Post> repo, ICrudRepository<User> usersCrudRepo,
        ITokenService tokenService, IUsersService usersService, IS3FileService s3FileService, ICrudRepository<PostWatcher> postWatchersRepo) : IPostsService
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

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid ownerId, int pageIndex, Guid userWatchedId, int pageSize = 20)
        {
            var posts = await repo.GetQueryable<Post>()
                .AsNoTracking()
                .Where(p => p.UserId == ownerId)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var tasks = posts.SelectMany(post =>
            {
                return new[]
                {
                    post.UpdateContentUrlIfNeededAsync(s3FileService, repo),
                    post.User.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo),
                    post.UpdateViewedCountAsync(repo, postWatchersRepo, post.Id, userWatchedId)
                };
            });

            await Task.WhenAll(tasks);

            return posts;
        }

        public async Task<IEnumerable<Post>> GetRecommendations(Guid userId, int pageIndex, int pageSize = 20)
        {
            var postsWatched = await repo.GetQueryable<PostWatcher>()
                .AsNoTracking()
                .Where(p => DateTime.UtcNow - p.CreatedAt <= TimeSpan.FromDays(14) && p.UserId == userId)
                .Select(p => p.PostId)
                .ToListAsync();

            var followingUserIds = await repo.GetQueryable<Follower>()
                .AsNoTracking()
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowingPersonId)
                .ToListAsync();

            var posts = await repo.GetQueryable<Post>()
                .AsNoTracking()
                .Where(p => DateTime.UtcNow - p.CreatedAt <= TimeSpan.FromDays(14)
                            && !postsWatched.Contains(p.Id)) 
                .Include(p => p.User)
                .OrderByDescending(p => followingUserIds.Contains(p.UserId))
                .ThenByDescending(p => p.LikesCount)
                .ThenByDescending(p => p.ViewedCount)
                .ThenByDescending(p => p.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var tasks = posts.SelectMany(post =>
            {
                return new[]
                {
                    post.UpdateContentUrlIfNeededAsync(s3FileService, repo),
                    post.User.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo),
                    post.UpdateViewedCountAsync(repo, postWatchersRepo, post.Id, userId)
                };
            });

            await Task.WhenAll(tasks);

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

        public async Task<Post> GetPostByIdAsync(Guid postId, Guid userId)
        {
            var post = await repo.GetQueryable<Post>()
               .Include(p => p.User)
               .SingleOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                throw new KeyNotFoundException($"Post with id {postId} not found");

            await Task.WhenAll(post.UpdateContentUrlIfNeededAsync(s3FileService, repo),
                post.User.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo),
                post.UpdateViewedCountAsync(repo, postWatchersRepo, post.Id, userId));

            return post;
        }

        private async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await repo.GetQueryable<Post>()
               .Include(p => p.User)
               .SingleOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                throw new KeyNotFoundException($"Post with id {postId} not found");

            await Task.WhenAll(post.UpdateContentUrlIfNeededAsync(s3FileService, repo),
                post.User.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo));

            return post;
        }
    }
}
