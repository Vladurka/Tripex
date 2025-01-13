using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Enums;

namespace Tripex.Core.Services
{
    public class LikesService(ICrudRepository<Like> repo, ICrudRepository<Post> postsRepo, 
        ICrudRepository<User> usersCrudRepo, IS3FileService s3FileService) : ILikesService
    {
        public async Task<ResponseOptions> AddLikeAsync(Like likeAdd)
        {
            var likeGet = await repo.GetQueryable<Like>()
                .SingleOrDefaultAsync(l => l.UserId == likeAdd.UserId && l.PostId == likeAdd.PostId);

            var post = await postsRepo.GetByIdAsync(likeAdd.PostId);

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

        public async Task<Like> GetLikeAsync(Guid id)
        {
            var like = await repo.GetQueryable<Like>()
                .Where(like => like.Id == id)
                .Include(like => like.User)
                .SingleOrDefaultAsync();

            if (like == null)
                throw new KeyNotFoundException($"Like with id {id} not found");

            await like.User.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo);

            return like;
        }

        public async Task<IEnumerable<Like>> GetLikesByPostIdAsync(Guid postId, int pageIndex, int pageSize = 20)
        {
            var likes = await repo.GetQueryable<Like>()
                .AsNoTracking()
                .Where(like => like.PostId == postId)
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

            var post = await postsRepo.GetByIdAsync(like.PostId);

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
    }
}
