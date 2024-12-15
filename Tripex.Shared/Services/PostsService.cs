using Tripex.Application.DTOs.Post;
using Tripex.Core.Domain.Entities;
using Tripex.Core.Domain.Interfaces.Repositories;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Core.Services
{
    public class PostsService(ICrudRepository<Post> repo) : IPostsService
    {
        public async Task<ResponseOptions> AddPostAsync(PostAdd postAdd)
        {
            var post = new Post(postAdd.UserId, postAdd.ContentUrl, postAdd.Description);

            await repo.AddAsync(post);
            return ResponseOptions.Ok;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var post = await repo.GetByIdAsync(postId);

            if (post == null)
                throw new KeyNotFoundException($"Post with id {postId} not found");

            post.Comments = await repo.GetByPostIdAsync<Comment>(post.Id);
            post.Likes = await repo.GetByPostIdAsync<Like>(post.Id);

            return post;
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId)
        {
            var posts = await repo.GetByUserIdAsync<Post>(userId);

            foreach (var post in posts)
            {
                post.Comments = await repo.GetByPostIdAsync<Comment>(post.Id);
                post.Likes = await repo.GetByPostIdAsync<Like>(post.Id);
            }
            return posts;
        }
    }
}
