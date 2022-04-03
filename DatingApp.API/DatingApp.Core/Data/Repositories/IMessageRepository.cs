using DatingApp.Core.Model;
using DatingApp.Core.Model.DTOs;

namespace DatingApp.Core.Data.Repositories
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string ConnectionId);
        Task<Group> GetMessageGroup(string groupName);
        void AddMessage(Message message);
        void DeleteMessage(int messageId, MessageDeletionOption messageDeletionOption);
        GetMessageDto EditMessage(int messageId, string newContent);
        Task<IEnumerable<GetMessageDto>> GetMessageThread(int senderId, int recepientId);
    }
}
