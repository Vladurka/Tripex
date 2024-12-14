using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Services
{
    public class PostsService(ICrudRepository<Post> postsRepo, ICrudRepository<User> usersRepo, ICrudRepository<Comment> commentsRepo, ICrudRepository<Like> likesRepo) : IPostsService
    {
        public async Task<ResponseOptions> AddPostAsync(Post post)
        {
            var user = await usersRepo.GetByIdAsync(post.UserId);

            if (user == null)
                throw new KeyNotFoundException($"User with id {post.UserId} not found");
           
            await postsRepo.AddAsync(post);
            return ResponseOptions.Ok;
        }

        public async Task<ResponseOptions> RemovePostByIdAsync(Guid postId)
        {
            var post = await postsRepo.GetByIdAsync(postId);

            if (post == null)
                return ResponseOptions.NotFound;

            await postsRepo.RemoveAsync(postId);
            return ResponseOptions.Ok;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await postsRepo.GetByIdAsync(postId);

            if (post == null)
                throw new KeyNotFoundException($"Post with id {postId} not found");

            post.Comments = await commentsRepo.GetByPostKeyAsync<Comment>(post.Id);
            post.Likes = await likesRepo.GetByPostKeyAsync<Like>(post.Id);

            return post;
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId)
        {
            var posts = await postsRepo.GetByUserKeyAsync<Post>(userId);

            foreach (var post in posts)
            {
                post.Comments = await commentsRepo.GetByPostKeyAsync<Comment>(post.Id);
                post.Likes = await commentsRepo.GetByPostKeyAsync<Like>(post.Id);
            }
            return posts;
        }
    }
}
