namespace Tripex.Core.Domain.Interfaces
{
    public interface IChatClient
    {
        public Task ReceiveMessage(string message);
    }
}
