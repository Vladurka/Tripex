using Tripex.Core.Domain.Entities;

namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string body);
        public string WelcomeMessage(User user);
    }
}
