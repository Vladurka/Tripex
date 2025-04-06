namespace Posts.Application.Data;

public interface IPostRepository
{
    public Task AddPostAsync(PostDb post);
    public Task<Post?> GetPostByIdAsync(PostId id);
    public Task<IEnumerable<Post>> GetAllPostsByUserAsync(ProfileId id);
    public Task<IEnumerable<Guid>> GetPostIdsByUserAsync(ProfileId id);
    public Task<IEnumerable<Post>> GetAllPostsAsync();
    public Task DeletePostAsync(PostId id);
    public Task IncrementPostCount(Guid profileId);
    public Task<int> GetPostCount(Guid profileId);
}