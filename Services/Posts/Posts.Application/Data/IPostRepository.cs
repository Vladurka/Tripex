namespace Posts.Application.Data;

public interface IPostRepository
{
    public Task SaveAsync(Post post);
    public Task<Post?> GetByIdAsync(PostId id);
    public Task<IEnumerable<Post>> GetAllByUserAsync(ProfileId id);
    public Task<IEnumerable<Post>> GetAllAsync();
    public Task DeleteAsync(PostId id);
}