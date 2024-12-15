using Tripex.Application.DTOs;
using Tripex.Application.DTOs.Like;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Services
{
    public class LikesService(ICrudRepository<Like> likesRepo) : ILikesService
    {
        public async Task<ResponseOptions> AddLike(LikeAdd likeAdd)
        {
            var likeGet = await likesRepo.GetByPostAndUserIdAsync<Like>(likeAdd.PostId, likeAdd.UserId);

            if(likeGet != null)
                return ResponseOptions.Exists;

            var like = new Like(likeAdd.UserId, likeAdd.PostId);

            await likesRepo.AddAsync(like);
            return ResponseOptions.Ok;
        }

        public async Task<Like> GetLike(Guid id)
        {
            var like = await likesRepo.GetByIdAsync(id);

            if(like == null)
                throw new KeyNotFoundException($"Like with id {id} not found");

            return like;
        }

        public async Task<IEnumerable<Like>> GetLikesByUserIdAsync(Guid userId)
        {
            var likes = await likesRepo.GetByUserIdAsync<Like>(userId);

            if (likes.Count() == 0)
                throw new KeyNotFoundException($"Likes with user id {userId} not found");

            return likes;
        }

        public async Task<IEnumerable<Like>> GetLikesByPostIdAsync(Guid postId)
        {
            var likes = await likesRepo.GetByPostIdAsync<Like>(postId);

            if (likes.Count() == 0)
                throw new KeyNotFoundException($"Likes with post id {postId} not found");

            return likes;
        }
    }
}
