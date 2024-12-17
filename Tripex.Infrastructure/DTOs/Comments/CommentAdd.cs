using System.ComponentModel.DataAnnotations;

namespace Tripex.Application.DTOs.Comments
{
    public class CommentAdd
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

        [StringLength(1)]
        public string Content { get; set; } = string.Empty;
    }
}
