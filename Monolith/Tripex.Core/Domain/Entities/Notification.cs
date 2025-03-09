namespace Tripex.Core.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public Guid AddresseeId { get; set; }
        public Guid EntityId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Notification() { }
        public Notification(Guid addresseeId, Guid entityId, string userName, string message) 
        {
            AddresseeId = addresseeId;
            EntityId = entityId;
            UserName = userName;
        }
        public Notification(Guid addresseeId, string message)
        {
            AddresseeId = addresseeId;
            Message = message;
        }

        public void GenerateLikePostNotificationAsync() =>
           Message = $"{UserName} liked your post";

        public void StartedFollowNotificationAsync() =>
           Message = $"{UserName} started to follow your";
    }
}
