using Microsoft.AspNetCore.Mvc;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class PostsController(IPostsService service) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddPost(Post post)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await service.AddPostAsync(post);
            return Ok();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Post>> GetPostById(Guid id)
        {
            return Ok(await service.GetPostByIdAsync(id));
        }

        [HttpGet("more/{userId:Guid}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsById(Guid userId)
        {
            return Ok(await service.GetPostsByUserIdAsync(userId));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeletePost(Guid id)
        {
            return CheckResponse(await service.RemovePostByIdAsync(id));
        }
    }
}
