using Microsoft.AspNetCore.Mvc;
using Tripex.Application.DTOs.Likes;
using Tripex.Application.DTOs.Posts;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class PostsController(IPostsService service, ICrudRepository<Post> repo) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> AddPost(PostAdd postAdd)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = new Post(postAdd.UserId, postAdd.ContentUrl, postAdd.Description);

            await repo.AddAsync(post);
            return Ok();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<PostGet>> GetPostById(Guid id)
        {
            var post = await service.GetPostByIdAsync(id);
            var postGet = new PostGet(post);
            return Ok(postGet);
        }

        [HttpGet("more/{userId:Guid}")]
        public async Task<ActionResult<IEnumerable<PostGet>>> GetPostsById(Guid userId)
        {
            var posts = await service.GetPostsByUserIdAsync(userId);
            var postsGet = posts.Select(post => new PostGet(post));

            return Ok(postsGet);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeletePost(Guid id)
        {
            return CheckResponse(await repo.RemoveAsync(id));
        }
    }
}
