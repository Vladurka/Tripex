using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Likes;
using Tripex.Application.DTOs.Posts;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class LikesController(ILikesService service, ICrudRepository<Like> repo) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddLike(LikeAdd likeAdd)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var like = new Like(likeAdd.UserId, likeAdd.PostId);
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
            var likesGet = likes.Select(like => new LikeGet(like));

            return Ok(likesGet);
        }

        [HttpGet("more/user/{userId:Guid}")]
        public async Task<ActionResult<IEnumerable<LikeGet>>> GetLikesByUser(Guid userId)
        {
            var likes = await service.GetLikesByUserIdAsync(userId);
            var likesGet = likes.Select(like => new LikeGet(like));

            return Ok(likesGet);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteLike(Guid id)
        {
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
