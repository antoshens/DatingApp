using Microsoft.AspNetCore.Http;
using PhotoService.Core.Events;

namespace Domain.Events
{
    public class PhotoAddedEvent : Event
    {
        public IFormFile File { get; set; }
    }
}
