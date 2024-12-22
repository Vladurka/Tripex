using System.ComponentModel.DataAnnotations;
using Tripex.Core.Domain.Interfaces.Contracts;

namespace Tripex.Core.Domain.Entities
{
    public class Post : BaseEntity, IUserForeignKey
    {
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        [Url]
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }

        public IEnumerable<Like> Likes { get; set; } = new List<Like>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        public Post() { }
        public Post(string contentUrl, string? description)
        {
            ContentUrl = contentUrl;
            Description = description;
        }
    }
}
