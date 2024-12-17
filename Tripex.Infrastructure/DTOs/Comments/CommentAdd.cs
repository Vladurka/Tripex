using System.ComponentModel.DataAnnotations;

namespace Tripex.Application.DTOs.Comments
{
    public class CommentAdd
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
