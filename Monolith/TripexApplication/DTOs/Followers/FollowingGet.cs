namespace Tripex.Application.DTOs.Followers
{
    public class FollowingGet : BaseEntity
    {
        public UserGetMin Following { get; set; }

        public FollowingGet(Follower follower)
        {
            if (follower.FollowingEntity == null)
                throw new ArgumentNullException(nameof(follower));

            Id = follower.Id;
            CreatedAt = follower.CreatedAt;

            var followingGet = new UserGetMin(follower.FollowingEntity);
            Following = followingGet;
        }
    }
}
