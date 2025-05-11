namespace BuildingBlocks.Messaging.Events.Profiles;

public record AddFollowEvent : IntegrationEvent
{
    public Guid ProfileId{ get; set; }
    public Guid FollowerId{ get; set; }
}