using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Services
{
    public class CommentsService(ICrudRepository<Comment> repo) : ICommentsService
    {
        public async Task<Comment> GetComment(Guid id)
        {
            var comment = await repo.GetQueryable<Comment>()
               .Where(comment => comment.Id == id)
               .Include(comment => comment.User)
               .SingleOrDefaultAsync();

            if (comment == null)
                throw new KeyNotFoundException($"Comment with id {id} not found");

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(Guid userId)
        {
            var comments = await repo.GetQueryable<Comment>()
                .Where(comment => comment.UserId == userId)
                .Include(comment => comment.User)
                .ToListAsync();

            return comments;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid postId)
        {
            var comments = await repo.GetQueryable<Comment>()
                .Where(comment => comment.PostId == postId) 
                .Include(comment => comment.User)
                .ToListAsync();

            return comments;
        }
    }
}
