using Tripex.Core.Domain.Interfaces;

namespace Tripex.Core.Domain.Entities
{
    public class Watcher<T> : BaseEntity where T : BaseEntity, IWatchable
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid EntityId { get; set; }
        public T? Entity { get; set; }

        public Watcher() { }
        public Watcher(Guid userId, Guid postId)
        {
            UserId = userId;
            EntityId = postId;
        }
    }
}
