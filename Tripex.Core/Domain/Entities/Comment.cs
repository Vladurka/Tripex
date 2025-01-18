using System.ComponentModel.DataAnnotations;
using Tripex.Core.Domain.Interfaces;

namespace Tripex.Core.Domain.Entities
{
    public class Comment : BaseEntity, ILikable
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid PostId { get; set; }
        public Post? Post { get; set; }

        public string Content { get; set; } = string.Empty;
        public int LikesCount { get; set; } = 0;

        public IEnumerable<Like<Comment>> Likes { get; set; } = new List<Like<Comment>>();

        public Comment() { }
        public Comment(Guid userId, Guid postId, string content)
        {
            UserId = userId;
            PostId = postId;
            Content = content;
        }
    }
}
