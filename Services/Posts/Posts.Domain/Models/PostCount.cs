namespace Posts.Domain.Models;

public class PostCount : Entity<Guid>
{
    public ProfileId ProfileId { get; private set; }
    public int Count { get; private set; }
    
    private PostCount() { }
    
    public static PostCount Create(ProfileId profileId)
    {
        return new PostCount
        {
            Id = Guid.NewGuid(),
            ProfileId = profileId,
            Count = 1
        };
    }

    public void AddCount() =>
        Count++;
}