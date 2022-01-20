using PhotoService.Core.Events;

namespace PhotoService.Core.Bus
{
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : Event;
        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}