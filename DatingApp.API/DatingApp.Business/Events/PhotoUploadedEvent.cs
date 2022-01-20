using DatingApp.Core.Events;

namespace DatingApp.Business.Events
{
    public class PhotoUploadedEvent : BaseEvent
    {
        public int UserId { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}
