using Microsoft.AspNetCore.Mvc;
using Tripex.Core.Domain.Interfaces.Services;

namespace Tripex.Controllers
{
    public class EmailNotificationsController(IEmailService emailService) : BaseApiController
    {
        [HttpPost("{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            await emailService.SendEmailAsync(email, "Welcome", "DIMA IS FUCKING NIGGA");
            return Ok();
        }
    }
}
