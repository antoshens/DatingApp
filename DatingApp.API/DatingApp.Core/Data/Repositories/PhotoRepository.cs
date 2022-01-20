using AutoMapper;
using DatingApp.Core.Model;
using DatingApp.Core.Model.DTOs;

namespace DatingApp.Core.Data.Repositories
{
    public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DataContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public PhotoDto AddPhoto(Photo photo)
        {
            Db.Photos.Add(photo);

            SaveAll();

            var photoModel = Mapper.Map<Photo, PhotoDto>(photo);
            return photoModel;
        }

        public async Task DeletePhoto(Photo photo)
        {
            Db.Photos.Remove(photo);

            await SaveAllAsync();
        }

        public IEnumerable<PhotoDto> GetAllPhotosByUserId(int userId)
        {
            var userPhotos = Db.Photos.Where(p => p.UserId == userId);

            var userPhotoModel = userPhotos.Select(p => Mapper.Map<Photo, PhotoDto>(p));

            return userPhotoModel;
        }

        public Photo GetPhoto(int photoId)
        {
           var photo = Db.Photos.First(p => p.PhotoId == photoId);

            return photo;
        }

        public T GetPhoto<T>(int photoId)
        {
            var photo = Db.Photos.First(p => p.PhotoId == photoId);
            var photoModel = Mapper.Map<Photo, T>(photo);

            return photoModel;
        }
    }
}
