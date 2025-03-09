namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IPostsService
    {
        public Task AddPostAsync(Post post);
        public Task<ResponseOptions> DeletePostAsync(Guid id);
        public Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId, int pageIndex, Guid userWatched, int pageSize = 20);
        public Task<IEnumerable<Post>> GetRecommendations(Guid userId, int pageIndex, int pageSize = 20);
        public Task<Post> GetPostByIdAsync(Guid postId, Guid userId);
        public Task<Post> GetPostByIdAsync(Guid postId);
    }
}
