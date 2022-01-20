using DatingApp.Core.Events;

namespace DatingApp.Core.Bus
{
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : BaseEvent;
        void Subscribe<T, TH>()
            where T : BaseEvent
            where TH : IEventHandler<T>;
    }
}
