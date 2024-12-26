using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Core.Services
{
    public class LikesService(ICrudRepository<Like> repo, ICrudRepository<Post> postsRepo) : ILikesService
    {
        public async Task<ResponseOptions> AddLikeAsync(Like likeAdd)
        {
            var likeGet = await repo.GetQueryable<Like>()
                .SingleOrDefaultAsync(l => l.UserId == likeAdd.UserId && l.PostId == likeAdd.PostId);

            if(likeGet != null)
                return ResponseOptions.Exists;

            await repo.AddAsync(likeAdd);

            var post = await postsRepo.GetByIdAsync(likeAdd.PostId);

            if(post == null)
                return ResponseOptions.NotFound;

            post.LikesCount++;
            await postsRepo.UpdateAsync(post);
            return ResponseOptions.Ok;
        }

        public async Task<Like> GetLikeAsync(Guid id)
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
