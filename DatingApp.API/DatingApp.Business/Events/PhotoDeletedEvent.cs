using DatingApp.Core.Events;

namespace DatingApp.Business.Events
{
    public class PhotoDeletedEvent : BaseEvent
    {
        public int PhotoId { get; set; }
    }
}
