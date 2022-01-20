using DatingApp.Business.Events;
using DatingApp.Business.Services;
using DatingApp.Core.Bus;

namespace DatingApp.Business.EventHandlers
{
    public class PhotoDeletedEventHandler : IEventHandler<PhotoDeletedEvent>
    {
        private readonly IPhotoService _photoService;

        public PhotoDeletedEventHandler(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        public async Task Handle(PhotoDeletedEvent @event)
        {
            await _photoService.RemovePhotoAsync(@event.PhotoId);
        }
    }
}
