using PhotoService.Core.Events;

namespace PhotoService.Business.Events
{
    public class PhotoAddedEvent : Event
    {
        public int UserId { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public bool IsMain { get; set; }
    }
}
