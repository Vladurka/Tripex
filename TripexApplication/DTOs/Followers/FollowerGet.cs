namespace Tripex.Application.DTOs.Followers
{
    public class FollowerGet
    {
        public Guid Id { get; set; }
        public UserGetMin Follower { get; set; }
        public string CreatedAt { get; set; } = string.Empty;

        public FollowerGet(Follower follower)
        {
            Id = follower.Id;
            CreatedAt = follower.CreatedAt.Humanize();

            var followerGet = new UserGetMin(follower.FollowerEntity!);
            Follower = followerGet;
        }
    }
}