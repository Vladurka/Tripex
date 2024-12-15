using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface ICommentsService
    {
        public Task<Comment> GetComment(Guid id);
        public Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId);
        public Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid postId);
    }
}
