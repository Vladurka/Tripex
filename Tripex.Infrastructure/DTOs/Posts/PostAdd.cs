using System.ComponentModel.DataAnnotations;

namespace Tripex.Application.DTOs.Posts
{
    public class PostAdd
    {
        [Required]
        public Guid UserId { get; set; }

        [Url]
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
