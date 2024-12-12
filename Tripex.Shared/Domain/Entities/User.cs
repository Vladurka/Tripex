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

        [StringLength(16, MinimumLength = 8)]
        public string Pass { get; set; } = string.Empty;

        public User(string userName, string email, string pass) 
        { 
            UserName = userName;
            Email = email;
            Pass = pass;
        }
    }
}
