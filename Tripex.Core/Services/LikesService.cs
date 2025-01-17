using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Enums;

namespace Tripex.Core.Services
{
    public class LikesService<T>(ICrudRepository<Like<T>> repo, ICrudRepository<Post> postsRepo, ICrudRepository<Comment> commentsRepo,
        ICrudRepository<User> usersCrudRepo, IS3FileService s3FileService) : ILikesService<T> where T : class, ILikable
    {
        public async Task<ResponseOptions> AddLikeToPostAsync(Like<T> likeAdd)
        {
            var likeGet = await repo.GetQueryable<Like<T>>()
                .SingleOrDefaultAsync(l => l.UserId == likeAdd.UserId && l.EntityId == likeAdd.EntityId);

            var post = await postsRepo.GetByIdAsync(likeAdd.EntityId);

            await using var transaction = await repo.BeginTransactionAsync();

            try
            {
                if (post == null)
                    return ResponseOptions.NotFound;

                if (likeGet != null)
                    return ResponseOptions.Exists;

                else
                {
                    await repo.AddAsync(likeAdd);
                    post.LikesCount++;
                    await postsRepo.UpdateAsync(post);
                }
                await transaction.CommitAsync();

                return ResponseOptions.Ok;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ResponseOptions> AddLikeToCommentAsync(Like<T> likeAdd)
        {
            var likeGet = await repo.GetQueryable<Like<T>>()
                .SingleOrDefaultAsync(l => l.UserId == likeAdd.UserId && l.EntityId == likeAdd.EntityId);

            var comment = await commentsRepo.GetByIdAsync(likeAdd.EntityId);

            await using var transaction = await repo.BeginTransactionAsync();

            try
            {
                if (comment == null)
                    return ResponseOptions.NotFound;

                if (likeGet != null)
                    return ResponseOptions.Exists;

                else
                {
                    await repo.AddAsync(likeAdd);
                    comment.LikesCount++;
                    await commentsRepo.UpdateAsync(comment);
                }
                await transaction.CommitAsync();

                return ResponseOptions.Ok;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Like<T>> GetEntityLikeAsync(Guid id)
        {
            var like = await repo.GetQueryable<Like<T>>()
                .Where(like => like.Id == id)
                .Include(like => like.User)
                .SingleOrDefaultAsync();

            if (like == null)
                throw new KeyNotFoundException($"Like with id {id} not found");

            await like.User.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo);

            return like;
        }

        public async Task<IEnumerable<Like<T>>> GetLikesByEntityIdAsync(Guid postId, int pageIndex, int pageSize = 20)
        {
            var likes = await repo.GetQueryable<Like<T>>()
                .AsNoTracking()
                .Where(like => like.EntityId == postId)
                .Include(like => like.User)
                .OrderBy(like => like.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var tasks = likes.Select(like => like.User.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo));
            await Task.WhenAll(tasks);

            return likes;
        }

        public async Task<ResponseOptions> DeletePostLikeAsync(Guid id)
        {
            var like = await repo.GetByIdAsync(id);

            if (like == null)
                return ResponseOptions.NotFound;

            var post = await postsRepo.GetByIdAsync(like.EntityId);

            if (post == null)
                return ResponseOptions.NotFound;

            await using var transaction = await repo.BeginTransactionAsync();

            try
            {
                await repo.RemoveAsync(id);
                post.LikesCount--;
                await postsRepo.UpdateAsync(post);
                await transaction.CommitAsync();

                return ResponseOptions.Ok;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ResponseOptions> DeleteCommentLikeAsync(Guid id)
        {
            var like = await repo.GetByIdAsync(id);

            if (like == null)
                return ResponseOptions.NotFound;

            var comment = await commentsRepo.GetByIdAsync(like.EntityId);

            if (comment == null)
                return ResponseOptions.NotFound;

            await using var transaction = await repo.BeginTransactionAsync();

            try
            {
                await repo.RemoveAsync(id);
                comment.LikesCount--;
                await commentsRepo.UpdateAsync(comment);
                await transaction.CommitAsync();

                return ResponseOptions.Ok;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
