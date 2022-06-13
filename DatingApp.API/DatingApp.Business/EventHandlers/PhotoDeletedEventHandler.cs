using DatingApp.Business.CQRS;
using DatingApp.Business.CQRS.Photo.Commands;
using DatingApp.Business.Events;
using DatingApp.Core.Bus;

namespace DatingApp.Business.EventHandlers
{
    public class PhotoDeletedEventHandler : IEventHandler<PhotoDeletedEvent>
    {
        private readonly ICQRSMediator _mediator;

        public PhotoDeletedEventHandler(ICQRSMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(PhotoDeletedEvent @event)
        {
            await _mediator.CommandByParameter<DeleteUserPhotoCommand, Task, int>(@event.PhotoId);
        }
    }
}
