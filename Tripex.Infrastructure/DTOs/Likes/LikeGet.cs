using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Likes
{
    public class LikeGet : BaseEntity
    {
        public Guid PostId { get; set; }
        public UserGetMin User { get; set; }

        public LikeGet(Like like)
        {
            Id = like.Id;
            User = new UserGetMin(like.User);
            PostId = like.PostId;
            CreatedAt = like.CreatedAt;
        }
    }
}
