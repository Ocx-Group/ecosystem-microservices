using Ecosystem.Domain.Core.Commands;
using Ecosystem.Domain.Core.Events;
namespace Ecosystem.Domain.Core.Bus;

public interface IEventBus
{
    Task SendCommand<T>(T command) where T : Command;
    Task Publish<T>(T @event) where T : Event;
}