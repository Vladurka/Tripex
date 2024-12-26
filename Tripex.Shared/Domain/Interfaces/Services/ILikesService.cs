using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface ILikesService
    {
        public Task<ResponseOptions> AddLikeAsync(Like like);
        public Task<Like> GetLikeAsync(Guid id);
        public Task<IEnumerable<Like>> GetLikesByPostIdAsync(Guid postId, int pageIndex, int pageSize = 20);
        public Task<ResponseOptions> DeleteLikeAsync(Guid id);
    }
}
