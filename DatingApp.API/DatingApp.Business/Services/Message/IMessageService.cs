namespace DatingApp.Business.Services.Message
{
    public interface IMessageService
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string ConnectionId);
        Task<Group> GetMessageGroup(string groupName);
        BusinessResponse<Core.Model.Message> CreateMessage(int senderId, SendMessageDto newMessage);
        bool DeleteMessage(GetMessageDto message, MessageDeletionOption messageDeletionOption);
        GetMessageDto EditMessage(int messageId, string newContent);
        Task<IEnumerable<GetMessageDto>> GetMessageThread(int senderId, int recepientId);
        Task AddConnectionToGroup(string groupName, Connection connection);
    }
}
