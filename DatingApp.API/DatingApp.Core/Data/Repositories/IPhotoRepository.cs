using DatingApp.Core.Model;
using DatingApp.Core.Model.DTOs;

namespace DatingApp.Core.Data.Repositories
{
    public interface IPhotoRepository : IBaseRepository<Photo>
    {
        PhotoDto AddPhoto(Photo photo);
        Task DeletePhoto(Photo photo);
        Photo GetPhoto(int photoId);
        T GetPhoto<T>(int photoId);
        IEnumerable<PhotoDto> GetAllPhotosByUserId(int userId);
    }
}
