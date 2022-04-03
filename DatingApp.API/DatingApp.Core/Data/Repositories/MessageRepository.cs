using AutoMapper;
using DatingApp.Core.Model;
using DatingApp.Core.Model.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Core.Data.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(DataContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public void AddGroup(Group group)
        {
            Db.Groups.Add(group);
        }

        public void RemoveConnection(Connection connection)
        {
            Db.Connections.Remove(connection);
        }

        public async Task<Connection> GetConnection(string ConnectionId)
        {
            return await Db.Connections.SingleOrDefaultAsync(c => c.ConnectionId == ConnectionId);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await Db.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(g => g.Name == groupName);
        }

        public void AddMessage(Message message)
        {
            Db.Messages.Add(message);
        }

        public GetMessageDto EditMessage(int messageId, string newContent)
        {
            if (messageId == 0)
            {
                throw new ArgumentNullException(nameof(messageId));
            }

            var message = Db.Messages.Single(m => m.MessageId == messageId);

            message.Content = newContent;
            message.DateEdited = DateTime.UtcNow;

            return Mapper.Map<GetMessageDto>(message);
        }

        public void DeleteMessage(int messageId, MessageDeletionOption messageDeletionOption)
        {
            if (messageId == 0)
            {
                throw new ArgumentNullException(nameof(messageId));
            }

            var storedMessage = Db.Messages.Single(m => m.MessageId == messageId);

            switch (messageDeletionOption)
            {
                case MessageDeletionOption.ForSender:
                    storedMessage.DeleteForSender();
                    Db.SaveChanges();

                    break;
                case MessageDeletionOption.ForRecepient:
                    storedMessage.DeleteForRecepient();
                    Db.SaveChanges();

                    break;
                case MessageDeletionOption.ForEveryone:
                    storedMessage.DeleteForEveryone();
                    Db.SaveChanges();

                    break;
                default: break;
            }
        }

        public async Task<IEnumerable<GetMessageDto>> GetMessageThread(int senderId, int recepientId)
        {
            var thread = await Db.Messages
                .Include(x => x.SenderUser).ThenInclude(_ => _.Photos)
                .Include(x => x.RecipientUser).ThenInclude(_ => _.Photos)
                .Where(m => m.SenderId == senderId && m.RecipientId == recepientId && !m.IsDeletedForSender && !m.IsDeletedForSender)
                .OrderBy(x => x.DateSent)
                .ToListAsync();

            var threadModel = thread.Select(m => Mapper.Map<GetMessageDto>(m));

            return threadModel;
        }
    }
}
