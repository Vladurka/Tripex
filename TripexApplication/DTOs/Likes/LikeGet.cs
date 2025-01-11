using Humanizer;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Likes
{
    public class LikeGet
    {
        public Guid Id { get; set; }
        public UserGetMin User { get; set; }
        public string CreatedAt { get; set; } = string.Empty;

        public LikeGet(Like like)
        {
            Id = like.Id;
            User = new UserGetMin(like.User);
            CreatedAt = like.CreatedAt.Humanize();
        }
    }
}
