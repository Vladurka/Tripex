using Microsoft.AspNetCore.Mvc;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class CommentsController(ICommentsService service, ICrudRepository<Comment> repo) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> GetComment(Comment comment)
        {
            await repo.AddAsync(comment);
            return Ok();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Comment>> GetComment(Guid id)
        {
            return Ok(await service.GetComment(id));
        }

        [HttpGet("more/post/{postId:Guid}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByPost(Guid postId)
        {
            return Ok(await service.GetCommentsByPostIdAsync(postId));
        }

        [HttpGet("more/user/{userId:Guid}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByUser(Guid userId)
        {
            return Ok(await service.GetCommentsByUserIdAsync(userId));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
