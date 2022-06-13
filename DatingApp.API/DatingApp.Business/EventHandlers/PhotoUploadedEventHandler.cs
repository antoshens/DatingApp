using DatingApp.Business.CQRS;
using DatingApp.Business.CQRS.Photo.Commands;
using DatingApp.Business.Events;
using DatingApp.Core.Bus;

namespace DatingApp.Business.EventHandlers
{
    public class PhotoUploadedEventHandler : IEventHandler<PhotoUploadedEvent>
    {
        private readonly ICQRSMediator _mediator;

        public PhotoUploadedEventHandler(ICQRSMediator mediator)
        {
            _mediator = mediator;
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

            _mediator.CommandByParameter<AddPhotoCommand, PhotoDto, PhotoDto>(photoModel);

            return Task.CompletedTask;
        }
    }
}
