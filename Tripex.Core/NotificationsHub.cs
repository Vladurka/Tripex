using Microsoft.AspNetCore.SignalR;
using Tripex.Core.Domain.Interfaces;

namespace Tripex.Core
{
    public sealed class NotificationsHub : Hub<IChatClient>
    {
        public async Task SendMessageToUser(string userId, string message)
        {
            await Clients.User(userId).ReceiveMessage(message);
        }
    }
}
