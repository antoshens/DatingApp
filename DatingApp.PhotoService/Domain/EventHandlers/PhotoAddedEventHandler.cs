using Domain.Events;
using PhotoService.Business.Interfaces;
using PhotoService.Core.Bus;

namespace Domain.EventHandlers
{
    public class PhotoAddedEventHandler : IEventHandler<PhotoAddedEvent>
    {
        private readonly IPhotoService _photoService;

        public PhotoAddedEventHandler(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        public async Task Handle(PhotoAddedEvent @event)
        {
            await _photoService.AddPhotoAsync(@event.File);
        }
    }
}
