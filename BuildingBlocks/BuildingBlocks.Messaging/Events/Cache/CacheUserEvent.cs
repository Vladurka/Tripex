namespace BuildingBlocks.Messaging.Events.Cache;

public record CacheUserEvent : IntegrationEvent
{
    public Guid ProfileId { get; set; }
}