namespace DatingApp.Business.Services.Message
{
    public class MessageService : BaseService, IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(DataContext db, IMessageRepository messageRepository) : base(db)
        {
            _messageRepository = messageRepository;
        }

        public void CreateMessage(int senderId, SendMessageDto newMessage)
        {
            var message = new Core.Model.Message(senderId, newMessage.RecepientId, newMessage.Content);

            _messageRepository.AddMessage(message);
        }

        public bool DeleteMessage(GetMessageDto message, MessageDeletionOption messageDeletionOption)
        {
            try
            {
                if (message is null)
                    throw new ArgumentNullException(nameof(message));

                _messageRepository.DeleteMessage(message.Id, messageDeletionOption);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public GetMessageDto EditMessage(GetMessageDto message, string newContent)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            var newMessage = _messageRepository.EditMessage(message.Id, newContent);

            return newMessage;
        }

        public Task<IEnumerable<GetMessageDto>> GetMessageThread(int senderId, int recepientId)
        {
            return _messageRepository.GetMessageThread(senderId, recepientId);
        }
    }
}
