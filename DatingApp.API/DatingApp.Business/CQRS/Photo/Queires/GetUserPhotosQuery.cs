namespace DatingApp.Business.CQRS.Photo.Queires
{
    public class GetUserPhotosQuery : IQueryHandler,
        IQueryByParameterHandler<IEnumerable<PhotoDto>, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserPhotosQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PhotoDto> HandleQuery(int userId)
        {
            return _unitOfWork.PhotoRepository.GetAllPhotosByUserId(userId);
        }
    }
}
