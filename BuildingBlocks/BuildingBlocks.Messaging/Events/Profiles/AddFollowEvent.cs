namespace BuildingBlocks.Messaging.Events.Profiles;

public record AddFollowEvent : IntegrationEvent
{
    public Guid ProfileId{ get; init; }
    public Guid FollowerId{ get; init; }
}