﻿namespace Tripex.Application.DTOs.Users
{
    public class UserRegister
    {
        [StringLength(25, MinimumLength = 2)]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(16, MinimumLength = 8)]
        public string Pass { get; set; } = string.Empty;

        [Compare("Pass")]
        public string ConfirmPass { get; set; } = string.Empty;
    }
}
