using System.Collections;
using Tripex.Application.DTOs.Comments;
using Tripex.Application.DTOs.Posts;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Users
{
    public class UserGet : BaseEntity
    {
        public IEnumerable<PostGetMin> Posts { get; set; } = new List<PostGetMin>();
        public string UserName { get; set; } = string.Empty;
        public UserGet(User user)
        {
            Id = user.Id;
            CreatedAt = user.CreatedAt;
            Posts = user.Posts
                .Select(post => new PostGetMin(post));

            UserName = user.UserName;
        }
    }
}
