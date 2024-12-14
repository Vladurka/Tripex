using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tripex.Core.Domain.Interfaces.Contracts;

namespace Tripex.Core.Domain.Entities
{
    public class Post : BaseEntity, IUserForeignKey
    {
        [Required]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        
        [Url]
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }

        public IEnumerable<Like> Likes { get; set; } = new List<Like>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        public int LikesCount => Likes.Count();
        public int CommentsCount => Comments.Count();
    }
}
