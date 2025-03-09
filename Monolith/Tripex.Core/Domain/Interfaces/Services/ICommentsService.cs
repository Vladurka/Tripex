using Tripex.Core.Domain.Entities;
using Tripex.Core.Enums;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface ICommentsService
    {
        public Task<ResponseOptions> AddCommentAsync(Comment comment);
        public Task<Comment> GetCommentAsync(Guid id);
        public Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid postId, int pageIndex, int pageSize = 20);
        public Task<ResponseOptions> DeleteCommentAsync(Guid id);
    }
}
