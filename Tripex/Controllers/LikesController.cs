using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Like;
using Tripex.Application.DTOs.Post;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class LikesController(ILikesService service, ICrudRepository<Like> repo) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddLike(LikeAdd like)
        {
            return CheckResponse(await service.AddLike(like));
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Like>> GetLike(Guid id)
        {
            return Ok(await service.GetLike(id));
        }

        [HttpGet("more/post/{postId:Guid}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikesByPost(Guid postId)
        {
            return Ok(await service.GetLikesByPostIdAsync(postId));
        }

        [HttpGet("more/user/{userId:Guid}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikesByUser(Guid userId)
        {
            return Ok(await service.GetLikesByUserIdAsync(userId));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteLike(Guid id)
        {
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
