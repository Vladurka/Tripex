using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Enums;

namespace Tripex.Core.Services
{
    public class LikesService<T>(ICrudRepository<Like<T>> repo, ICrudRepository<T> entityRepo,
        ICrudRepository<User> usersCrudRepo, IS3FileService s3FileService) : ILikesService<T> where T : BaseEntity, ILikable
    {
        public async Task<ResponseOptions> AddLikeAsync(Like<T> likeAdd)
        {
            var likeGet = await repo.GetQueryable<Like<T>>()
                .SingleOrDefaultAsync(l => l.UserId == likeAdd.UserId && l.EntityId == likeAdd.EntityId);

            var entity = await entityRepo.GetByIdAsync(likeAdd.EntityId);

            await using var transaction = await repo.BeginTransactionAsync();

            try
            {
                if (entity == null)
                    return ResponseOptions.NotFound;

                if (likeGet != null)
                    return ResponseOptions.Exists;

                else
                {
                    await repo.AddAsync(likeAdd);
                    entity.LikesCount++;
                    await entityRepo.UpdateAsync(entity);
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

        public async Task<ResponseOptions> DeleteLikeAsync(Guid id)
        {
            var like = await repo.GetByIdAsync(id);

            if (like == null)
                return ResponseOptions.NotFound;

            var post = await entityRepo.GetByIdAsync(like.EntityId);

            if (post == null)
                return ResponseOptions.NotFound;

            await using var transaction = await repo.BeginTransactionAsync();

            try
            {
                await repo.RemoveAsync(id);
                post.LikesCount--;
                await entityRepo.UpdateAsync(post);
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
