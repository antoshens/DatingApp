using DatingApp.Business.Events;
using DatingApp.Core.Bus;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebAPI.Controllers
{
    [Route("api/photo")]
    public class PhotoApiController : BaseApiController
    {
        private readonly IEventBus _eventBus;
        private readonly IPhotoService _photoService;
        private readonly ICurrentUser _user;

        public PhotoApiController(IEventBus eventBus, IPhotoService photoService, ICurrentUser user)
        {
            _eventBus = eventBus;
            _photoService = photoService;
            _user = user;
        }

        [Route("upload/{isMain}"), HttpPost]
        public async Task UploadNewPhoto(IFormFile file, bool isMain)
        {
            var buffer = await _photoService.CreatePhotoByteArrayAsync(file);

            var currentUserId = _user.GetCurrentUserId();

            if (!currentUserId.HasValue)
            {
                throw new ArgumentException("Unable to indentify a user");
            }

            _eventBus.Publish(new PhotoAddedEvent
            {
                File = buffer,
                FileName = file.FileName,
                IsMain = isMain,
                UserId = currentUserId.Value
            });
        }

        [Route("remove"), HttpDelete]
        public async Task RemovePhoto(PhotoDto photoModel)
        {
            _eventBus.Publish(new DeletePhotoEvent
            {
                PhotoId = photoModel.PhotoId,
                PublicId = photoModel.PublicId
            });
        }
    }
}
