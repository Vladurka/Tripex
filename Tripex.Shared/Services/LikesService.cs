using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Services
{
    public class LikesService(ICrudRepository<Like> repo) : ILikesService
    {
        public async Task<ResponseOptions> AddLike(Like likeAdd)
        {
            var likeGet = await repo.GetQueryable<Like>()
                .SingleOrDefaultAsync(l => l.UserId == likeAdd.UserId && l.PostId == likeAdd.PostId);

            if(likeGet != null)
                return ResponseOptions.Exists;

            await repo.AddAsync(likeAdd);
            return ResponseOptions.Ok;
        }

        public async Task<Like> GetLike(Guid id)
        {
            var like = await repo.GetQueryable<Like>()
                .Where(like => like.Id == id)
                .Include(like => like.User)
                .SingleOrDefaultAsync();

            if (like == null)
                throw new KeyNotFoundException($"Like with id {id} not found");

            return like;
        }

        public async Task<IEnumerable<Like>> GetLikesByUserIdAsync(Guid userId)
        {
            var likes = await repo.GetQueryable<Like>()
                .Where(like => like.UserId == userId)
                .Include(like => like.User)
                .ToListAsync();

            return likes;
        }

        public async Task<IEnumerable<Like>> GetLikesByPostIdAsync(Guid postId)
        {
            var likes = await repo.GetQueryable<Like>()
                .Where(like => like.PostId == postId)
                .Include(like => like.User)
                .ToListAsync();

            return likes;
        }
    }
}
