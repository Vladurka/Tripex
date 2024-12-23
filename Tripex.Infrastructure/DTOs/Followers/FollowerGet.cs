using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Followers
{
    public class FollowerGet : BaseEntity
    {
        public UserGetMin Follower { get; set; }

        public FollowerGet(Follower follower)
        {
            Id = follower.Id;
            CreatedAt = follower.CreatedAt;

            var followerGet = new UserGetMin(follower.FollowerEntity);
            Follower = followerGet;
        }
    }
}