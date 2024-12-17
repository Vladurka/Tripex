using System.ComponentModel.DataAnnotations;
using Tripex.Core.Domain.Interfaces.Contracts;


namespace Tripex.Core.Domain.Entities
{
    public class Comment : BaseEntity, IUserForeignKey, IPostForeignKey
    {
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public Guid PostId { get; set; }
        public Post? Post { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public Comment(Guid userId, Guid postId, string content)
        {
            UserId = userId;
            PostId = postId;
            Content = content;
        }
    }
}
