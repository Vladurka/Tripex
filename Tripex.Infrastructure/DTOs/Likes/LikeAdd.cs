using System.ComponentModel.DataAnnotations;

namespace Tripex.Application.DTOs.Likes
{
    public class LikeAdd
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid PostId { get; set; }
    }
}
