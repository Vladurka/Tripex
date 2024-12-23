using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Likes;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Controllers
{
    [Authorize]
    public class LikesController(ILikesService service, ICrudRepository<Like> repo,
        ITokenService tokenService) : BaseApiController
    {
        [HttpPost("{postId:Guid}")]
        public async Task<ActionResult> AddLike(Guid postId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = tokenService.GetUserIdByToken();

            var like = new Like(id, postId);
            return CheckResponse(await service.AddLike(like));
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<LikeGet>> GetLike(Guid id)
        {
            var like = await service.GetLike(id);
            var likeGet = new LikeGet(like);
            return Ok(likeGet);
        }

        [HttpGet("more/post/{postId:Guid}")]
        public async Task<ActionResult<IEnumerable<LikeGet>>> GetLikesByPost(Guid postId)
        {
            var likes = await service.GetLikesByPostIdAsync(postId);
            var likesGet = likes.Select(like => new LikeGet(like))
                .ToList();

            return Ok(likesGet);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteLike(Guid id)
        {
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
