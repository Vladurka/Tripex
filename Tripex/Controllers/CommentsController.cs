using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Comments;
using Tripex.Application.DTOs.Likes;
using Tripex.Application.DTOs.Posts;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class CommentsController(ICommentsService service, ICrudRepository<Comment> repo) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddComment(CommentAdd commentAdd)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = new Comment(commentAdd.UserId, commentAdd.PostId, commentAdd.Content);

            await repo.AddAsync(comment);
            return Ok();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CommentGet>> GetLike(Guid id)
        {
            var comment = await service.GetComment(id);
            var commentGet = new CommentGet(comment);
            return Ok(commentGet);
        }

        [HttpGet("more/post/{postId:Guid}")]
        public async Task<ActionResult<IEnumerable<CommentGet>>> GetLikesByPost(Guid postId)
        {
            var comments = await service.GetCommentsByPostIdAsync(postId);
            var commentsGet = comments.Select(comments => new CommentGet(comments));

            return Ok(commentsGet);
        }

        [HttpGet("more/user/{userId:Guid}")]
        public async Task<ActionResult<IEnumerable<CommentGet>>> GetLikesByUser(Guid userId)
        {
            var comments = await service.GetCommentsByUserIdAsync(userId);
            var commentsGet = comments.Select(comments => new CommentGet(comments));

            return Ok(commentsGet);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
