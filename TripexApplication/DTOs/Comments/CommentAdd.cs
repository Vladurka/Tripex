using System.ComponentModel.DataAnnotations;

namespace Tripex.Application.DTOs.Comments
{
    public class CommentAdd
    {
        public Guid PostId { get; set; }

        [MinLength(1)]
        public string Content { get; set; } = string.Empty;
    }
}
