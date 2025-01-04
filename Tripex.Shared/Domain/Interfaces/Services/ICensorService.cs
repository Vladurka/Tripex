using Microsoft.AspNetCore.Http;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface ICensorService
    {
        public Task<string> CheckImageAsync(IFormFile photo);
        public Task<string> CheckTextAsync(string text);
    }
}
