namespace DatingApp.Business.CQRS.Message.Queires
{
    public class GetMessageThreadQuery : IQueryHandler,
        IQueryByTwoParametersHandler<Task<IEnumerable<GetMessageDto>>, int, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMessageThreadQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<GetMessageDto>> HandleQuery(int senderId, int recepientId)
        {
            return _unitOfWork.MessageRepository.GetMessageThread(senderId, recepientId);
        }
    }
}
