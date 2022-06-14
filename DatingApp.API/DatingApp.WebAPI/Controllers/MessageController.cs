using DatingApp.Business.CQRS.Message.Queires;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;

namespace DatingApp.WebAPI.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : BaseApiController
    {
        private readonly ICQRSMediator _mediator;
        private readonly ICurrentUser _currentUser;

        public MessageController(ICQRSMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        //[Route("send"), HttpPost]
        //public void CreateMessage(SendMessageDto message)
        //{
        //    _messageService.CreateMessage(_currentUser.UserId, message);
        //}

        //[Route("delete/{messageDeletionOption}"), HttpDelete]
        //public void DeleteMessage(
        //    [FromBody]GetMessageDto message,
        //    [FromRoute]MessageDeletionOption messageDeletionOption)
        //{
        //    _messageService.DeleteMessage(message, messageDeletionOption);
        //}

        //[Route("edit/{messageId}"), HttpPut]
        //public GetMessageDto EditMessage(int messageId, string content)
        //{
        //    var newMessage = _messageService.EditMessage(messageId, content);

        //    return newMessage;
        //}

        [Route("thread"), HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<GetMessageDto>> GetThread([FromODataUri]int recepientId)
        {
            var thread = await _mediator.QueryByTwoParameters<GetMessageThreadQuery, Task<IEnumerable<GetMessageDto>>, int, int>(_currentUser.UserId, recepientId);

            return thread;
        }
    }
}
