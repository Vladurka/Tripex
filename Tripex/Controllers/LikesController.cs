using Tripex.Application.DTOs.Likes;

namespace Tripex.Controllers
{
    [Authorize]
    public class LikesController(ILikesService<Post> postLikesService, ILikesService<Comment> commentLikesService,
        ITokenService tokenService) : BaseApiController
    {
        [HttpPost("post/{postId:guid}")]
        public async Task<ActionResult> AddLikeToPost(Guid postId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = tokenService.GetUserIdByToken();

            var like = new Like<Post>(id, postId);
            return CheckResponse(await postLikesService.AddLikeAsync(like));
        }

        [HttpPost("comment/{commentId:guid}")]
        public async Task<ActionResult> AddLikeToComment(Guid commentId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = tokenService.GetUserIdByToken();

            var like = new Like<Comment>(id, commentId);
            return CheckResponse(await commentLikesService.AddLikeAsync(like));
        }

        [HttpGet("post/{id:guid}")]
        public async Task<ActionResult<LikeGet<Post>>> GetPostLike(Guid id)
        {
            var like = await postLikesService.GetEntityLikeAsync(id);
            var likeGet = new LikeGet<Post>(like);
            return Ok(likeGet);
        }

        [HttpGet("comment/{id:guid}")]
        public async Task<ActionResult<LikeGet<Comment>>> GetCommentLike(Guid id)
        {
            var like = await commentLikesService.GetEntityLikeAsync(id);
            var likeGet = new LikeGet<Comment>(like);
            return Ok(likeGet);
        }

        [HttpGet("more/post/{postId:guid}/{pageIndex:int}")]
        public async Task<ActionResult<IEnumerable<LikeGet<Post>>>> GetLikesByPost(Guid postId, int pageIndex)
        {
            var likes = await postLikesService.GetLikesByEntityIdAsync(postId, pageIndex);
            var likesGet = likes.Select(like => new LikeGet<Post>(like))
                .ToList();

            return Ok(likesGet);
        }

        [HttpGet("more/comment/{commentId:guid}/{pageIndex:int}")]
        public async Task<ActionResult<IEnumerable<LikeGet<Comment>>>> GetLikesByComment(Guid commentId, int pageIndex)
        {
            var likes = await commentLikesService.GetLikesByEntityIdAsync(commentId, pageIndex);
            var likesGet = likes.Select(like => new LikeGet<Comment>(like))
                .ToList();

            return Ok(likesGet);
        }

        [HttpDelete("post/{id:guid}")]
        public async Task<ActionResult> DeletePostLike(Guid id)
        {
            return CheckResponse(await postLikesService.DeleteLikeAsync(id));
        }

        [HttpDelete("comment/{id:guid}")]
        public async Task<ActionResult> DeleteCommentLike(Guid id)
        {
            return CheckResponse(await commentLikesService.DeleteLikeAsync(id));
        }
    }
}
