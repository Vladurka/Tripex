namespace Tripex.Application.DTOs.Likes
{
    public class LikeGet<T> where T : BaseEntity, ILikable
    {
        public Guid Id { get; set; }
        public UserGetMin User { get; set; }
        public string CreatedAt { get; set; } = string.Empty;

        public LikeGet(Like<T> like)
        {
            Id = like.Id;
            User = new UserGetMin(like.User!);
            CreatedAt = like.CreatedAt.Humanize();
        }
    }
}
