using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Post;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class PostsController(IPostsService service, ICrudRepository<Post> repo) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddPost(PostAdd post)
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
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
