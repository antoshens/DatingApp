using DatingApp.Business.CQRS.Photo.Commands;
using DatingApp.Business.CQRS.Photo.Queires;
using DatingApp.Business.Events;
using DatingApp.Core.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;

namespace DatingApp.WebAPI.Controllers
{
    [Route("api/photo")]
    public class PhotoApiController : BaseApiController
    {
        private readonly IEventBus _eventBus;
        private readonly ICQRSMediator _mediator;
        private readonly ICurrentUser _user;

        public PhotoApiController(IEventBus eventBus, ICQRSMediator mediator, ICurrentUser user)
        {
            _eventBus = eventBus;
            _mediator = mediator;
            _user = user;
        }

        [Route("upload/{isMain}"), HttpPost]
        public async Task UploadNewPhoto(IFormFile file, bool isMain)
        {
            var buffer = await _mediator.CommandByParameter<AddPhotoCommand, Task<byte[]>, IFormFile>(file);

            _eventBus.Publish(new PhotoAddedEvent
            {
                File = buffer,
                FileName = file.FileName,
                IsMain = isMain,
                UserId = _user.UserId
            });
        }

        [Route("remove"), HttpDelete]
        public void RemovePhoto(PhotoDto photoModel)
        {
            _eventBus.Publish(new DeletePhotoEvent
            {
                PhotoId = photoModel.PhotoId,
                PublicId = photoModel.PublicId
            });
        }

        [Route("userPhotos"), HttpGet]
        [EnableQuery]
        public IEnumerable<PhotoDto> GetPhotos([FromODataUri] int userId)
        {
            return _mediator.QueryByParameter<GetUserPhotosQuery, IEnumerable<PhotoDto>, int>(userId);
        }
    }
}
