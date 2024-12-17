using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Services
{
    public class LikesService(ICrudRepository<Like> repo, IUsersService usersService, IPostsService postsService) : ILikesService
    {
        public async Task<ResponseOptions> AddLike(Like likeAdd)
        {
            var likeGet = await repo.GetByPostAndUserIdAsync<Like>(likeAdd.PostId, likeAdd.UserId);

            if(likeGet != null)
                return ResponseOptions.Exists;

            await repo.AddAsync(likeAdd);
            return ResponseOptions.Ok;
        }

        public async Task<Like> GetLike(Guid id)
        {
            var like = await repo.GetByIdAsync(id);

            if(like == null)
                throw new KeyNotFoundException($"Like with id {id} not found");

            like.User = await usersService.GetUserByIdAsync(like.UserId);

            return like;
        }

        public async Task<IEnumerable<Like>> GetLikesByUserIdAsync(Guid userId)
        {
            var likes = await repo.GetQueryable<Like>()
                .Where(like => like.UserId == userId)
                .Include(like => like.User)
                .ToListAsync();

            if (!likes.Any())
                throw new KeyNotFoundException($"Likes with user id {userId} not found");

            return likes;
        }

        public async Task<IEnumerable<Like>> GetLikesByPostIdAsync(Guid postId)
        {
            var post = await postsService.GetPostByIdAsync(postId);
            var likes = await repo.GetQueryable<Like>()
                .Where(like => like.PostId == post.Id)
                .Include(like => like.User)
                .ToListAsync();

            if (!likes.Any())
                throw new KeyNotFoundException($"Likes with post id {postId} not found");

            return likes;
        }
    }
}
