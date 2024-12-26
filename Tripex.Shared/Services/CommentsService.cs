using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Services
{
    public class CommentsService(ICrudRepository<Comment> repo, ICrudRepository<Post> postsRepo) : ICommentsService
    {
        public async Task<ResponseOptions> AddCommentAsync(Comment commentAdd)
        {
            await repo.AddAsync(commentAdd);

            var post = await postsRepo.GetByIdAsync(commentAdd.PostId);

            if (post == null)
                return ResponseOptions.NotFound;

            post.CommentsCount++;
            await postsRepo.UpdateAsync(post);
            return ResponseOptions.Ok;
        }

        public async Task<Comment> GetCommentAsync(Guid id)
        {
            var comment = await repo.GetQueryable<Comment>()
               .Include(comment => comment.User)
               .SingleOrDefaultAsync(comment => comment.Id == id);

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
