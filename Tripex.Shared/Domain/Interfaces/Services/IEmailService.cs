namespace Tripex.Core.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
