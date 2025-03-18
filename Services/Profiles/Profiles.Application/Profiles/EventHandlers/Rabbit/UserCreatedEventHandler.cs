using BuildingBlocks.Messaging.Events.Profiles;
using MassTransit;
using MediatR;

namespace Profiles.Application.Profiles.EventHandlers.Rabbit;

public class UserCreatedEventHandler : IConsumer<CreateProfileEvent>
{
    public Task Consume(ConsumeContext<CreateProfileEvent> context)
    {
        
    }
}