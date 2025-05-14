namespace Posts.Application.Data;

public interface IPostRepository
{
    public Task AddPostAsync(PostDb post);
    public Task<Post?> GetPostByIdAsync(PostId id);
    public Task<IEnumerable<Post>> GetPostsByUserAsync(ProfileId profileId);
    public Task<IEnumerable<Guid>> GetPostIdsByUserAsync(ProfileId profileId);
    public Task<IEnumerable<Post>> GetAllPostsAsync();
    public Task DeletePostAsync(PostId id, ProfileId profileId);
    public Task DeletePostsAsync(ProfileId profileId);
    public Task DeletePostCountAsync(ProfileId profileId);
    public Task<int> GetPostCount(ProfileId profileId);
}