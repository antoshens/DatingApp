using Microsoft.AspNetCore.Http;

namespace DatingApp.Business.Services
{
    public interface IPhotoService
    {
        PhotoDto UploadNewPhoto(PhotoDto photoModel);
        Task<PhotoDto> RemovePhotoAsync(int photoId);
        Task<byte[]> CreatePhotoByteArrayAsync(IFormFile file);
        IEnumerable<PhotoDto> GetUserPhotos(int userId);
    }
}
