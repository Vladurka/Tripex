using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;
using OneOf;

namespace Tripex.Core.Services
{
    public class PostsService(ICrudRepository<Post> repo, ICrudRepository<User> crudUserRepo, IUsersRepository userRepo) : IPostsService
    {
        public async Task<ResponseOptions> AddPost(Post post)
        {
            var user = crudUserRepo.GetByIdAsync(post.UserId);

            if (user == null)
                return ResponseOptions.BadRequest;

            await repo.AddAsync(post);
            return ResponseOptions.Ok;
        }

        public async Task<ResponseOptions> RemovePost(Guid postId)
        {
            var post = await repo.GetByIdAsync(postId);

            if (post == null)
                return ResponseOptions.NotFound;

            await repo.RemoveAsync(postId);
            return ResponseOptions.Ok;
        }
    }
}
