using PhotoService.Core.Events;

namespace PhotoService.Business.Events
{
    public class DeletePhotoEvent : Event
    {
        public int PhotoId { get; set; }
        public string PublicId { get; set; }
    }
}
