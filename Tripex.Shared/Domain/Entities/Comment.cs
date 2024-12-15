using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tripex.Core.Domain.Interfaces.Contracts;
using System.Text.Json.Serialization;

namespace Tripex.Core.Domain.Entities
{
    public class Comment : BaseEntity, IUserForeignKey, IPostForeignKey
    {
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        public Guid PostId { get; set; }

        [JsonIgnore]
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
