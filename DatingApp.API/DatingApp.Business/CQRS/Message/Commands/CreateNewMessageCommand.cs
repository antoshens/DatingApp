namespace DatingApp.Business.CQRS.Message.Commands
{
    public class CreateNewMessageCommand : ICommandHandler,
        ICommandByTwoParametersHandler<Core.Model.Message, int, SendMessageDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateNewMessageCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Core.Model.Message HandleCommand(int senderId, SendMessageDto newMessage)
        {
            if (senderId == 0 || newMessage.RecepientId == 0)
            {
                throw new Exception("Can not send the message to the given recepient");
            }

            if (senderId == newMessage.RecepientId)
            {
                throw new Exception("You can not send message to yourself");
            }

            var message = new Core.Model.Message(senderId, newMessage.RecepientId, newMessage.Content);

            try
            {
                _unitOfWork.MessageRepository.AddMessage(message);
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Can not send the message to the given recepient");
            }

            return message;
        }
    }
}
