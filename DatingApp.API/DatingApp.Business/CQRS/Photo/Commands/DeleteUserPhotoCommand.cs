namespace DatingApp.Business.CQRS.Photo.Commands
{
    public class DeleteUserPhotoCommand : ICommandHandler,
        ICommandByParameterHandler<Task, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserPhotoCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleCommand(int photoId)
        {
            var photo = _unitOfWork.PhotoRepository.GetPhoto(photoId);

            if (photo is null)
            {
                throw new ArgumentException("Couldn't find a photo to delete");
            }

            await _unitOfWork.PhotoRepository.DeletePhoto(photo);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
