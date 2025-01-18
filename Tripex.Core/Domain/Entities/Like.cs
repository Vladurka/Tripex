namespace Tripex.Core.Domain.Entities
{
    public class Like<T> : BaseEntity where T : BaseEntity, ILikable
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid EntityId { get; set; }
        public T? Entity { get; set; }

        public Like() { }
        public Like(Guid userId, Guid postId)
        {
            UserId = userId;
            EntityId = postId;
        }
    }
}
