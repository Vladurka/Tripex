﻿namespace Tripex.Core.Services
{
    public class PostsService(ICrudRepository<Post> repo, ICrudRepository<User> usersCrudRepo,
        ITokenService tokenService, IUsersService usersService, IS3FileService s3FileService, ICrudRepository<Watcher<Post>> postWatchersRepo) : IPostsService
    {
        public async Task AddPostAsync(Post post) 
        {
            await using var transaction = await repo.BeginTransactionAsync();

            try
            {
                await repo.AddAsync(post);
                var user = await usersService.GetUserByIdAsync(post.UserId);

                user.PostsCount++;
                await usersCrudRepo.UpdateAsync(user);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
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
                    post.User?.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo),
                    post.UpdateViewedCountAsync(repo, postWatchersRepo, post.Id, userWatchedId)
                };
            });

            await Task.WhenAll(tasks!);

            return posts;
        }

        public async Task<IEnumerable<Post>> GetRecommendations(Guid userId, int pageIndex, int pageSize = 20)
        {
            var postsWatched = await repo.GetQueryable<Watcher<Post>>()
                .AsNoTracking()
                .Where(p => DateTime.UtcNow - p.CreatedAt <= TimeSpan.FromDays(14) && p.UserId == userId)
                .Select(p => p.EntityId)
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
                    post.User?.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo),
                    post.UpdateViewedCountAsync(repo, postWatchersRepo, post.Id, userId)
                };
            });

            await Task.WhenAll(tasks!);

            return posts;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId, Guid userId)
        {
            var post = await repo.GetQueryable<Post>()
               .Include(p => p.User)
               .SingleOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                throw new KeyNotFoundException($"Post with id {postId} not found");

            await Task.WhenAll(post.UpdateContentUrlIfNeededAsync(s3FileService, repo),
                post.User?.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo)!,
                post.UpdateViewedCountAsync(repo, postWatchersRepo, post.Id, userId));

            return post;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await repo.GetQueryable<Post>()
               .Include(p => p.User)
               .SingleOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                throw new KeyNotFoundException($"Post with id {postId} not found");

            await Task.WhenAll(post.UpdateContentUrlIfNeededAsync(s3FileService, repo),
                post.User?.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo)!);

            return post;
        }

        public async Task<ResponseOptions> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if(post.UserId != tokenService.GetUserIdByToken())
                return ResponseOptions.BadRequest;

            await using var transaction = await repo.BeginTransactionAsync();

            try
            {
                var user = await usersService.GetUserByIdAsync(post.UserId);
                user.PostsCount--;

                await s3FileService.DeleteFileAsync(postId.ToString());
                await repo.RemoveAsync(postId);

                await usersCrudRepo.UpdateAsync(user);
                await transaction.CommitAsync();

                return ResponseOptions.Ok;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
