namespace Posts.Application.Data;

public interface IPostRepository
{
    public Task AddAsync(PostDb post);
    public Task<Post?> GetByIdAsync(PostId id);
    public Task<IEnumerable<Post>> GetAllByUserAsync(ProfileId id);
    public Task<IEnumerable<Guid>> GetPostIdsByUserAsync(ProfileId id);
    public Task<IEnumerable<Post>> GetAllAsync();
    public Task DeleteAsync(PostId id);
}