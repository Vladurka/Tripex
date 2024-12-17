using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface ILikesService
    {
        public Task<ResponseOptions> AddLike(Like like);
        public Task<Like> GetLike(Guid id);
        public Task<IEnumerable<Like>> GetLikesByUserIdAsync(Guid userId);
        public Task<IEnumerable<Like>> GetLikesByPostIdAsync(Guid postId);
    }
}
