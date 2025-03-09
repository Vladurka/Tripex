using System.ComponentModel.DataAnnotations;

namespace Auth.API.DTOs
{
    public class RegisterDto
    {
        [StringLength(25, MinimumLength = 2)]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(16, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
