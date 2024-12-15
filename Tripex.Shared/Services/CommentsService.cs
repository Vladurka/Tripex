using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Services
{
    public class CommentsService(ICrudRepository<Comment> repo) : ICommentsService
    {
        public async Task<Comment> GetComment(Guid id)
        {
            var comment = await repo.GetByIdAsync(id);

            if (comment == null)
                throw new KeyNotFoundException($"Comment with id {id} not found");

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId)
        {
            var comments = await repo.GetByUserIdAsync<Comment>(userId);

            if (comments.Count() == 0)
                throw new KeyNotFoundException($"Comments with user id {userId} not found");

            return comments;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid postId)
        {
            var comments = await repo.GetByPostIdAsync<Comment>(postId);

            if (comments.Count() == 0)
                throw new KeyNotFoundException($"Comments with post id {postId} not found");

            return comments;
        }
    }
}
