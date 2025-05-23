namespace BuildingBlocks.Messaging.Events.Profiles;

public record DecrementPostCountEvent : IntegrationEvent
{
    public Guid ProfileId { get; init; }
}