using PhotoService.Business.Events;
using PhotoService.Business.Interfaces;
using PhotoService.Core.Bus;

namespace PhotoService.Business.EventHandlers
{
    public class PhotoAddedEventHandler : IEventHandler<PhotoAddedEvent>
    {
        private readonly IPhotoService _photoService;
        private readonly IEventBus _eventBus;

        public PhotoAddedEventHandler(IPhotoService photoService,
            IEventBus eventBus)
        {
            _photoService = photoService;
            _eventBus = eventBus;
        }

        public async Task Handle(PhotoAddedEvent @event)
        {
            var uploadPhotoResult = await _photoService.AddPhotoAsync(@event.File, @event.FileName);

            _eventBus.Publish<PhotoUploadedEvent>(new PhotoUploadedEvent
            {
                PublicId = uploadPhotoResult.PublicId,
                Url = uploadPhotoResult.Url.ToString(),
                IsMain = @event.IsMain,
                UserId = @event.UserId
            });
        }
    }
}
