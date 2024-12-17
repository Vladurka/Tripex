using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IPostsService
    {
        public Task<Post> GetPostByIdAsync(Guid postId);
        public Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId);
    }
}
