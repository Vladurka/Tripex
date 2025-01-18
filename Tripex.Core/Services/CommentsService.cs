using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;
using Tripex.Core.Enums;

namespace Tripex.Core.Services
{
    public class CommentsService(ICrudRepository<Comment> repo, ICrudRepository<Post> postsRepo,
        ITokenService tokenService, ICrudRepository<User> usersCrudRepo, IS3FileService s3FileService) : ICommentsService
    {
        public async Task<ResponseOptions> AddCommentAsync(Comment commentAdd)
        {
            await using var transaction = await repo.BeginTransactionAsync();
            try
            {
                await repo.AddAsync(commentAdd);

                var post = await postsRepo.GetByIdAsync(commentAdd.PostId);

                if (post == null)
                    return ResponseOptions.NotFound;

                post.CommentsCount++;
                await postsRepo.UpdateAsync(post);
                await transaction.CommitAsync();

                return ResponseOptions.Ok;
            }
            catch (Exception )
            {
                transaction.Rollback(); 
                throw;
            }
        }

        public async Task<Comment> GetCommentAsync(Guid id)
        {
            var comment = await repo.GetQueryable<Comment>()
               .Include(comment => comment.User)
               .SingleOrDefaultAsync(comment => comment.Id == id);

            if (comment == null)
                throw new KeyNotFoundException($"Comment with id {id} not found");

            await comment.User?.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo)!;

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid postId, int pageIndex, int pageSize = 20)
        {
            var comments = await repo.GetQueryable<Comment>()
                .AsNoTracking()
                .Where(comment => comment.PostId == postId) 
                .Include(comment => comment.User)
                .OrderBy(comments => comments.LikesCount)
                .ThenByDescending(comments => comments.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var tasks = comments.Select(c => c.User?.UpdateAvatarUrlIfNeededAsync(s3FileService, usersCrudRepo)!);
            await Task.WhenAll(tasks);

            return comments;
        }

        public async Task<ResponseOptions> DeleteCommentAsync(Guid id)
        {
            var comment = await repo.GetByIdAsync(id);

            if (comment == null)
                return ResponseOptions.NotFound;

            if (comment.UserId != tokenService.GetUserIdByToken())
                return ResponseOptions.BadRequest;

            await using var transaction = await repo.BeginTransactionAsync();

            try
            {
                await repo.RemoveAsync(id);

                var post = await postsRepo.GetByIdAsync(comment.PostId);

                if (post == null)
                    return ResponseOptions.NotFound;

                post.CommentsCount--;
                await postsRepo.UpdateAsync(post);
                await transaction.CommitAsync();

                return ResponseOptions.Ok;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
