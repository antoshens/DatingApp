using PhotoService.Core.Events;

namespace PhotoService.Business.Events
{
    public class PhotoUploadedEvent : Event
    {
        public int UserId { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}
