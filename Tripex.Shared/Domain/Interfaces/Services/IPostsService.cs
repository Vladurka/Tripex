using Tripex.Application.DTOs;
using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IPostsService
    {
        public Task<ResponseOptions> AddPostAsync(Post post);
        public Task<ResponseOptions> RemovePostByIdAsync(Guid postId);
        public Task<Post> GetPostByIdAsync(Guid postId);
        public Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId);
    }
}
