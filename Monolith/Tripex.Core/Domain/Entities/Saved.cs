namespace Tripex.Core.Domain.Entities
{
    public class Saved<T> : BaseEntity where T : BaseEntity, ISavable
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid EntityId { get; set; }
        public T? Entity { get; set; }

        public Saved() { }
        public Saved(Guid userId, Guid postId)
        {
            UserId = userId;
            EntityId = postId;
        }
    }
}
