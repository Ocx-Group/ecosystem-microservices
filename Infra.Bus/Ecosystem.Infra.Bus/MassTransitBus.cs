using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Commands;
using DomainEvent = Ecosystem.Domain.Core.Events.Event;
using MassTransit;
namespace Ecosystem.Infra.Bus;

public class MassTransitBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task SendCommand<T>(T command) where T : Command
    {
        return _publishEndpoint.Publish(command);
    }

    public Task Publish<T>(T @event) where T : DomainEvent
    {
        return _publishEndpoint.Publish(@event);
    }
}