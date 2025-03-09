namespace Tripex.Application.DTOs.Users
{
    public class UserLogin
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(16, MinimumLength = 8)]
        public string Pass { get; set; } = string.Empty;

        [Compare("Pass")]
        public string ConfirmPass { get; set; } = string.Empty;
    }
}
