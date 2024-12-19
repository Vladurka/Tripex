using Tripex.Application.DTOs.Comments;
using Tripex.Application.DTOs.Likes;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Posts
{
    public class PostGet : BaseEntity
    {
        public UserGetMin User { get; set; }
        public string ContentUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IEnumerable<LikeGet> Likes { get; set; } = new List<LikeGet>();
        public IEnumerable<CommentGet> Comments { get; set; } = new List<CommentGet>();

        public int LikesCount;
        public int CommentsCount;

        public PostGet(Post post)
        {
            Id = post.Id;
            CreatedAt = post.CreatedAt;
            User = new UserGetMin(post.User);
            ContentUrl = post.ContentUrl;
            Description = post.Description;

            Likes = post.Likes
                .Where(like => like.User != null)
                .Select(like => new LikeGet(like));

            Comments = post.Comments
                .Where(comment => comment.User != null)
                .Select(comment => new CommentGet(comment));

            LikesCount = Likes.Count();
            CommentsCount = Comments.Count();
        }
    }
}
