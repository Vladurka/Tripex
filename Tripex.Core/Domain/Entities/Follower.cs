using System.ComponentModel.DataAnnotations;

namespace Tripex.Core.Domain.Entities
{
    public class Follower : BaseEntity
    {
        [Required]
        public Guid FollowerId { get; set; }
        public User FollowerEntity { get; set; }

        [Required]
        public Guid FollowingPersonId { get; set; }
        public User FollowingEntity { get; set; }

        public Follower() { }
        public Follower(Guid followingPersonId, Guid followerId)
        {
            FollowingPersonId = followingPersonId;
            FollowerId = followerId;
        }
    }
}
