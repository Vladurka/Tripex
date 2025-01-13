namespace Tripex.Core.Domain.Entities
{
    public class PostWatcher : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public PostWatcher() { }
        public PostWatcher(Guid userId, Guid postId)
        {
            UserId = userId;
            PostId = postId;
        }
    }
}
