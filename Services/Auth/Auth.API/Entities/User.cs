using BuildingBlocks;

namespace Auth.API.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    public User() { }
    public User(string userName, string email, string password)
    {
        UserName = userName;
        Email = email;
        Password = password;
    }
}