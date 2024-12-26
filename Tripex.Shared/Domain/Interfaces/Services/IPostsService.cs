using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IPostsService
    {
        public Task AddPostAsync(Post post);
        public Task<ResponseOptions> DeletePostAsync(Guid id);
        public Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId, int pageIndex, int pageSize = 20);
    }
}
