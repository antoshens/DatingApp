using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Business.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PhotoService(IPhotoRepository photoRepo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _photoRepo = photoRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public PhotoDto UploadNewPhoto(PhotoDto photoModel)
        {
            var user = _unitOfWork.UserRepository.GetUser(photoModel.UserId);
            var photo = new Core.Model.Photo(photoModel.PublicId, photoModel.Url, photoModel.IsMain, user);

            var resPhoto = _photoRepo.AddPhoto(photo);

            return resPhoto;
        }

        public async Task<PhotoDto> RemovePhotoAsync(int photoId)
        {
            var photo = _photoRepo.GetPhoto(photoId);

            if (photo is null)
            {
                throw new ArgumentException("Couldn't find a photo to delete");
            }

            await _photoRepo.DeletePhoto(photo);

            return _mapper.Map<Photo, PhotoDto>(photo);
        }

        public async Task<byte[]> CreatePhotoByteArrayAsync(IFormFile file)
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

        public IEnumerable<PhotoDto> GetUserPhotos(int userId)
        {
           return _photoRepo.GetAllPhotosByUserId(userId);
        }
    }
}
