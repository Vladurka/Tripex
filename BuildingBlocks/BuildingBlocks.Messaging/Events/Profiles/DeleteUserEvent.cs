namespace BuildingBlocks.Messaging.Events.Profiles;

public record DeleteUserEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
}