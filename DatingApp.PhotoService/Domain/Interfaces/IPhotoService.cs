using CloudinaryDotNet.Actions;

namespace PhotoService.Business.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(byte[] file, string fileName);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
