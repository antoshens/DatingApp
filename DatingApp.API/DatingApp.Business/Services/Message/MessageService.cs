namespace DatingApp.Business.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public void AddGroup(Group group)
        {
            _messageRepository.AddGroup(group);
            _messageRepository.SaveAll();
        }

        public BusinessResponse<Core.Model.Message> CreateMessage(int senderId, SendMessageDto newMessage)
        {
            if (senderId == 0 || newMessage.RecepientId == 0)
            {
                return new BusinessResponse<Core.Model.Message>
                {
                    Failed = true,
                    FailedMessage = "Can not send the message to the given recepient"
                };
            }

            if (senderId == newMessage.RecepientId)
            {
                return new BusinessResponse<Core.Model.Message>
                {
                    Failed = true,
                    FailedMessage = "You can not send message to yourself"
                };
            }

            var message = new Core.Model.Message(senderId, newMessage.RecepientId, newMessage.Content);

            try
            {
                _messageRepository.AddMessage(message);
                _messageRepository.SaveAll();
            }
            catch (Exception ex)
            {
                return new BusinessResponse<Core.Model.Message>
                {
                    Failed = true,
                    FailedMessage = "Can not send the message to the given recepient"
                };
            }

            return new BusinessResponse<Core.Model.Message>
            {
                Data = message
            };
        }

        public bool DeleteMessage(GetMessageDto message, MessageDeletionOption messageDeletionOption)
        {
            try
            {
                if (message is null)
                    throw new ArgumentNullException(nameof(message));

                _messageRepository.DeleteMessage(message.Id, messageDeletionOption);
                _messageRepository.SaveAll();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public GetMessageDto EditMessage(int messageId, string newContent)
        {
            var newMessage = _messageRepository.EditMessage(messageId, newContent);
            _messageRepository.SaveAll();

            return newMessage;
        }

        public async Task<Connection> GetConnection(string ConnectionId)
        {
            return await _messageRepository.GetConnection(ConnectionId);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
           return await _messageRepository.GetMessageGroup(groupName);
        }

        public Task<IEnumerable<GetMessageDto>> GetMessageThread(int senderId, int recepientId)
        {
            return _messageRepository.GetMessageThread(senderId, recepientId);
        }

        public void RemoveConnection(Connection connection)
        {
            _messageRepository.RemoveConnection(connection);
            _messageRepository.SaveAll();
        }
    }
}
