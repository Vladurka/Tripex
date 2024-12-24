using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Posts;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using Tripex.Core.Domain.Interfaces.Services.Security;

namespace Tripex.Controllers
{
    [Authorize]
    public class PostsController(IPostsService service,
        ITokenService tokenService) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddPost(PostAdd postAdd)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = tokenService.GetUserIdByToken();

            var post = new Post(id, postAdd.ContentUrl, postAdd.Description);

            await service.AddPostAsync(post);  
            return Ok();
        }

        [HttpGet("more/{userId:Guid}")]
        public async Task<ActionResult<IEnumerable<PostGet>>> GetPostsById(Guid userId)
        {
            var posts = await service.GetPostsByUserIdAsync(userId);
            var postsGet = posts.Select(post => new PostGet(post))
                .ToList();

            return Ok(postsGet);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeletePost(Guid id)
        {
            return CheckResponse(await service.DeletePostAsync(id));
        }
    }
}
