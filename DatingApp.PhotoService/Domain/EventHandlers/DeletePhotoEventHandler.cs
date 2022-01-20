using PhotoService.Business.Events;
using PhotoService.Business.Interfaces;
using PhotoService.Core.Bus;

namespace PhotoService.Business.EventHandlers
{
    public class DeletePhotoEventHandler : IEventHandler<DeletePhotoEvent>
    {
        private readonly IPhotoService _photoService;
        private readonly IEventBus _eventBus;

        public DeletePhotoEventHandler(IPhotoService photoService, IEventBus eventBus)
        {
            _photoService = photoService;
            _eventBus = eventBus;
        }

        public async Task Handle(DeletePhotoEvent @event)
        {
            await _photoService.DeletePhotoAsync(@event.PublicId);

            _eventBus.Publish(new PhotoDeletedEvent
            {
                PhotoId = @event.PhotoId
            });
        }
    }
}
