namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface ICensorService
    {
        public Task<string> CheckTextAsync(string text);
    }
}
