using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Auth.API.DTOs
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(16, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
