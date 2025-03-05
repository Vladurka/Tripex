using Microsoft.AspNetCore.Identity;

namespace Auth.API;

public class AppUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
}