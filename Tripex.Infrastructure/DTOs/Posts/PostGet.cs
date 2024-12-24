using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Posts
{
    public class PostGet : BaseEntity
    {
        public UserGetMin User { get; set; }
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int LikesCount;
        public int CommentsCount;

        public PostGet(Post post)
        {
            Id = post.Id;
            CreatedAt = post.CreatedAt;
            User = new UserGetMin(post.User);
            ContentUrl = post.ContentUrl;
            Description = post.Description;
            
            LikesCount = post.LikesCount;
            CommentsCount = post.CommentsCount;
        }
    }
}
