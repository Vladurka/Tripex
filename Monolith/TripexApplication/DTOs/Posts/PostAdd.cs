using Microsoft.AspNetCore.Http;

namespace Tripex.Application.DTOs.Posts
{
    public class PostAdd
    {
        [Required]
        public required IFormFile Photo { get; set; }
        public string? Description { get; set; }
    }
}
