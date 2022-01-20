using DatingApp.Core.Events;

namespace DatingApp.Business.Events
{
    public class DeletePhotoEvent : BaseEvent
    {
        public int PhotoId { get; set; }
        public string PublicId { get; set; }
    }
}
