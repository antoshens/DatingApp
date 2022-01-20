using PhotoService.Core.Events;

namespace PhotoService.Business.Events
{
    public class PhotoDeletedEvent : Event
    {
        public int PhotoId { get; set; }
    }
}
