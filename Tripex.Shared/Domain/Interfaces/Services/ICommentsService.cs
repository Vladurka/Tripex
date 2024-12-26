using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface ICommentsService
    {
        public Task<ResponseOptions> AddCommentAsync(Comment comment);
        public Task<Comment> GetCommentAsync(Guid id);
        public Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId);
        public Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid postId);
    }
}
