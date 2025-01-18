using Tripex.Core.Domain.Entities;
using Tripex.Core.Enums;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface ILikesService<T> where T : class, ILikable
    {
        public Task<ResponseOptions> AddLikeAsync(Like<T> like);
        public Task<Like<T>> GetEntityLikeAsync(Guid id);
        public Task<IEnumerable<Like<T>>> GetLikesByEntityIdAsync(Guid postId, int pageIndex, int pageSize = 20);
        public Task<ResponseOptions> DeleteLikeAsync(Guid id);
    }
}
