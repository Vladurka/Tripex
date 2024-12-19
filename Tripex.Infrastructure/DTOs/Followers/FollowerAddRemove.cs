using System.ComponentModel.DataAnnotations;

namespace Tripex.Application.DTOs.Followers
{
    public class FollowerAddRemove
    {
        [Required]
        public Guid FollowerId { get; set; }

        [Required]
        public Guid FollowingPersonId { get; set; }
    }
}
