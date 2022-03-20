﻿namespace DatingApp.Business.Services.Message
{
    public interface IMessageService
    {
        void CreateMessage(int senderId, SendMessageDto newMessage);
        bool DeleteMessage(GetMessageDto message, MessageDeletionOption messageDeletionOption);
        GetMessageDto EditMessage(int messageId, string newContent);
        Task<IEnumerable<GetMessageDto>> GetMessageThread(int senderId, int recepientId);
    }
}