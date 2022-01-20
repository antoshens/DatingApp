using DatingApp.Business.Events;
using DatingApp.Business.Services;
using DatingApp.Core.Bus;

namespace DatingApp.Business.EventHandlers
{
    public class PhotoUploadedEventHandler : IEventHandler<PhotoUploadedEvent>
    {
        private readonly IPhotoService _photoService;

        public PhotoUploadedEventHandler(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        public Task Handle(PhotoUploadedEvent @event)
        {
            var photoModel = new PhotoDto
            {
                PublicId = @event.PublicId,
                Url = @event.Url,
                IsMain = @event.IsMain,
                UserId = @event.UserId
            };

            _photoService.UploadNewPhoto(photoModel);

            return Task.CompletedTask;
        }
    }
}
