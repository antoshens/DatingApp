using DatingApp.Core.Events;

namespace DatingApp.Business.Events
{
    public class PhotoAddedEvent : BaseEvent
    {
        public int UserId { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public bool IsMain { get; set; }
    }
}
