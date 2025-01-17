using System.ComponentModel.DataAnnotations;
using Tripex.Core.Domain.Interfaces;

namespace Tripex.Core.Domain.Entities
{
    public class Like<T> : BaseEntity where T : class, ILikable
    {
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public Guid EntityId { get; set; }
        public T Entity { get; set; }

        public Like() { }
        public Like(Guid userId, Guid postId)
        {
            UserId = userId;
            EntityId = postId;
        }
    }
}
