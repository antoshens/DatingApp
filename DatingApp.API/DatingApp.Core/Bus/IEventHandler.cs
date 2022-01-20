using DatingApp.Core.Events;

namespace DatingApp.Core.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : BaseEvent
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {

    }
}
