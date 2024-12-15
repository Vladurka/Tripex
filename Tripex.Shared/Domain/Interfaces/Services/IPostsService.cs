using Tripex.Application.DTOs.Post;
using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IPostsService
    {
        public Task<ResponseOptions> AddPostAsync(PostAdd post);
        public Task<Post> GetPostByIdAsync(Guid postId);
        public Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId);
    }
}
