using Tripex.Application.DTOs.Posts;

namespace Tripex.Controllers
{
    [Authorize]
    public class PostsController(IPostsService service, IS3FileService s3FileService,
        ITokenService tokenService, ICensorService censorService) : BaseApiController
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

            var userId = tokenService.GetUserIdByToken();
            var postId = Guid.NewGuid();

            if (!string.IsNullOrWhiteSpace(postAdd.Description))
            {
                var isAvailable = await censorService.CheckTextAsync(postAdd.Description);

                if (isAvailable != "No")
                    return BadRequest("Description is not available");
            }

            string photoUrl = await s3FileService.UploadFileAsync(postAdd.Photo, postId.ToString());

            var post = new Post(postId, userId, photoUrl, postAdd.Description);

            await service.AddPostAsync(post);  
            return Ok();
        }

        [HttpGet("{postId:guid}")]
        public async Task<ActionResult<PostGet>> GetPostsByUserId(Guid postId)
        {
            var post = await service.GetPostByIdAsync(postId, tokenService.GetUserIdByToken());

            return Ok(new PostGet(post));
        }

        [HttpGet("more/{userId:guid}/{pageIndex:int}")]
        public async Task<ActionResult<IEnumerable<PostGet>>> GetPostsByUserId(Guid userId, int pageIndex)
        {
            var posts = await service.GetPostsByUserIdAsync(userId, pageIndex, userId);

            var postsGet = posts.Select(post => new PostGet(post))
                .ToList();

            return Ok(postsGet);
        }

        [HttpGet("more/{pageIndex:int}")]
        public async Task<ActionResult<IEnumerable<PostGet>>> GetRecommendations(int pageIndex)
        {
            var posts = await service.GetRecommendations(tokenService.GetUserIdByToken(), pageIndex);

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
