using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Posts
{
    public class PostGetMin : BaseEntity
    {
        public UserGetMin User { get; set; }
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }

        public PostGetMin(Post post)
        {
            Id = post.Id;
            CreatedAt = post.CreatedAt;

            if (post.User == null)
                throw new ArgumentNullException(nameof(post.User), $"User is null");

            User = new UserGetMin(post.User);

            ContentUrl = post.ContentUrl;
            Description = post.Description;
        }
    }
}
