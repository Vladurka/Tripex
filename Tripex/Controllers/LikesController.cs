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
    public class LikesController(ILikesService service,
        ITokenService tokenService) : BaseApiController
    {
        [HttpPost("{postId:guid}")]
        public async Task<ActionResult> AddLike(Guid postId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = tokenService.GetUserIdByToken();

            var like = new Like(id, postId);
            return CheckResponse(await service.AddLikeAsync(like));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LikeGet>> GetLike(Guid id)
        {
            var like = await service.GetLikeAsync(id);
            var likeGet = new LikeGet(like);
            return Ok(likeGet);
        }

        [HttpGet("more/post/{postId:guid}/{pageIndex:int}")]
        public async Task<ActionResult<IEnumerable<LikeGet>>> GetLikesByPost(Guid postId, int pageIndex)
        {
            var likes = await service.GetLikesByPostIdAsync(postId, pageIndex);
            var likesGet = likes.Select(like => new LikeGet(like))
                .ToList();

            return Ok(likesGet);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteLike(Guid id)
        {
            return CheckResponse(await service.DeleteLikeAsync(id));
        }
    }
}
