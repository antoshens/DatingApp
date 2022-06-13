using Microsoft.AspNetCore.Http;

namespace DatingApp.Business.CQRS.Photo.Commands
{
    public class AddPhotoCommand : ICommandHandler,
        ICommandByParameterHandler<Task<byte[]>, IFormFile>,
        ICommandByParameterHandler<PhotoDto, PhotoDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddPhotoCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<byte[]> HandleCommand(IFormFile file)
        {
            var buffer = new byte[file.Length];

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (file.Length > 0)
            {
                buffer = stream.ToArray();
            }

            return buffer;
        }

        public PhotoDto HandleCommand(PhotoDto photoModel)
        {
            var user = _unitOfWork.UserRepository.GetUser(photoModel.UserId);
            var photo = new Core.Model.Photo(photoModel.PublicId, photoModel.Url, photoModel.IsMain, user);

            var resPhoto = _unitOfWork.PhotoRepository.AddPhoto(photo);

            _unitOfWork.SaveChanges();

            return resPhoto;
        }
    }
}
