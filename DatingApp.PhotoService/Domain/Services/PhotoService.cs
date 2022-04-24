using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PhotoService.Business.Interfaces;
using PhotoService.Business.Util;

namespace PhotoService.Business.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(CloudinarySettings config)
        {
            var account = new Account(
                config.CloudName,
                config.ApiKey,
                config.ApiSecret);

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<ImageUploadResult> AddPhotoAsync(byte[] file, string fileName)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                var stream = new MemoryStream(file);

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
