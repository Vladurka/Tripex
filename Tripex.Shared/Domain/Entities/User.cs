using System.ComponentModel.DataAnnotations;

namespace Tripex.Core.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? AvatarUrl { get; set; }

        [StringLength(25, MinimumLength = 2)]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MinLength(8)]
        public string Pass { get; set; } = string.Empty;
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public IEnumerable<Like> Likes { get; set; } = new List<Like>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public User() { }
        public User(string userName, string email, string pass)
        {
            UserName = userName;
            Email = email;
            Pass = pass;
        }
    }
}
