﻿using DatingApp.Business.Services.Message;
using DatingApp.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebAPI.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : BaseApiController
    {
        private readonly IMessageService _messageService;
        private readonly ICurrentUser _currentUser;

        public MessageController(IMessageService messageService, ICurrentUser currentUser)
        {
            _messageService = messageService;
            _currentUser = currentUser;
        }

        [Route("send"), HttpPost]
        public void CreateMessage(SendMessageDto message)
        {
            _messageService.CreateMessage(_currentUser.UserId, message);
        }

        [Route("delete/{messageDeletionOption}"), HttpDelete]
        public void DeleteMessage(
            [FromBody]GetMessageDto message,
            [FromRoute]MessageDeletionOption messageDeletionOption)
        {
            _messageService.DeleteMessage(message, messageDeletionOption);
        }

        [Route("edit/{messageId}"), HttpPatch]
        public GetMessageDto EditMessage(int messageId, string content)
        {
            var newMessage = _messageService.EditMessage(messageId, content);

            return newMessage;
        }

        [Route("thread/{recepientId}"), HttpGet]
        public async Task<IEnumerable<GetMessageDto>> GetThread(int recepientId)
        {
            var thread = await _messageService.GetMessageThread(_currentUser.UserId, recepientId);

            return thread;
        }
    }
}
