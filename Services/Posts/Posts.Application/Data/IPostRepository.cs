namespace Posts.Application.Data;

public interface IPostRepository
{
    public Task AddPostAsync(Post postById);
    public Task<Post?> GetPostByIdAsync(PostId id);
    public Task<IEnumerable<Post>> GetPostsByProfileAsync(ProfileId profileId);
    public Task<IEnumerable<Guid>> GetPostIdsByUserAsync(ProfileId profileId);
    public Task<IEnumerable<Post>> GetAllPostsAsync();
    public Task DeletePostAsync(PostId id);
    public Task DeletePostsAsync(ProfileId profileId);
}