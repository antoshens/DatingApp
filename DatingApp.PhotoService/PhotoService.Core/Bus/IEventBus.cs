using PhotoService.Core.Events;

namespace PhotoService.Core.Bus
{
    public interface IEventBus
    {
        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}