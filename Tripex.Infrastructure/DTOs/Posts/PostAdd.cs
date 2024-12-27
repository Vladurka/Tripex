using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tripex.Application.DTOs.Posts
{
    public class PostAdd
    {
        [Required]
        public required IFormFile Photo { get; set; }
        public string? Description { get; set; }
    }
}
