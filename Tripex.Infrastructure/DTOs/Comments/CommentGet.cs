using Humanizer;
using Tripex.Application.DTOs.Users;
using Tripex.Core.Domain.Entities;

namespace Tripex.Application.DTOs.Comments
{
    public class CommentGet
    {
        public Guid Id { get; set; }
        public UserGetMin User { get; set; }
        public string Content { get; set; } = string.Empty;
        public int LikesCount { get; set; }
        public string CreatedAt { get; set; } = string.Empty;

        public CommentGet(Comment comment) 
        {
            Id = comment.Id;
            User = new UserGetMin(comment.User);
            LikesCount = comment.LikesCount;
            Content = comment.Content;
            CreatedAt = comment.CreatedAt.Humanize();
        }
    }
}
