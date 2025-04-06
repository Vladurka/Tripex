namespace Posts.Domain.Models;

public class PostsCount : Entity<Guid>
{
    public ProfileId ProfileId { get; private set; }
    public int Count { get; private set; } = 0;
    
    private PostsCount() { }
    
    public static PostsCount Create(ProfileId profileId, int count)
    {
        return new PostsCount
        {
            Id = Guid.NewGuid(),
            ProfileId = profileId,
        };
    }
}