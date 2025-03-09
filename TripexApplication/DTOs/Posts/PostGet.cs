namespace Tripex.Application.DTOs.Posts
{
    public class PostGet
    {
        public Guid Id { get; set; }
        public UserGetMin User { get; set; }
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int LikesCount;
        public int CommentsCount;

        public string CreatedAt { get; set; } = string.Empty;

        public PostGet(Post post)
        {
            Id = post.Id;
            User = new UserGetMin(post.User!);
            ContentUrl = post.ContentUrl;
            Description = post.Description;
            
            LikesCount = post.LikesCount;
            CommentsCount = post.CommentsCount;

            CreatedAt = post.CreatedAt.Humanize();
        }
    }
}
