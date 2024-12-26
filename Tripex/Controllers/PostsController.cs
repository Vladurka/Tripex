using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Posts;
using Tripex.Core.Domain.Entities;
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
            if(!ModelState.IsValid)
            {
                string errors = string.Join("\n",
                ModelState.Values.SelectMany(value => value.Errors)
                    .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            var id = tokenService.GetUserIdByToken();

            var post = new Post(id, postAdd.ContentUrl, postAdd.Description);

            await service.AddPostAsync(post);  
            return Ok();
        }

        [HttpGet("more/{userId:guid}/{pageIndex:int}")]
        public async Task<ActionResult<IEnumerable<PostGet>>> GetPostsById(Guid userId, int pageIndex)
        {
            var posts = await service.GetPostsByUserIdAsync(userId, pageIndex);
            var postsGet = posts.Select(post => new PostGet(post))
                .ToList();

            return Ok(postsGet);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeletePost(Guid id)
        {
            return CheckResponse(await service.DeletePostAsync(id));
        }
    }
}
