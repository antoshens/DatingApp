using DatingApp.Business.Services.Authentication;
using DatingApp.Business.Services.Message;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.WebAPI.SignalR
{
    public class MessageHub : Hub
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public MessageHub(ITokenService tokenService, IMessageService messageService, IUserService userService)
        {
            _tokenService = tokenService;
            _messageService = messageService;
            _userService = userService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUserName = httpContext.Request.Query["user"].ToString();
            var currentUserName = _tokenService.GetCurrentUserName(Context.User);

            var groupName = GetGroupName(currentUserName, otherUserName);

            var currentUser = await _userService.GetUserByName(currentUserName);
            var otherUser = await _userService.GetUserByName(otherUserName);

            var messages = await _messageService.GetMessageThread(otherUser.Id, currentUser.Id);

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(SendMessageDto message)
        {
            var httpContext = Context.GetHttpContext();
            var otherUserName = httpContext.Request.Query["user"].ToString();
            var currentUserName = _tokenService.GetCurrentUserName(Context.User);

            var groupName = GetGroupName(currentUserName, otherUserName);

            var currentUser = await _userService.GetUserByName(currentUserName);

            var respose = _messageService.CreateMessage(currentUser.Id, message);

            if (respose.Failed) throw new HubException(respose.FailedMessage);

            await Clients.Group(groupName).SendAsync("NewMessage", respose.Data);
        }

        private string GetGroupName(string currentUserName, string otherUserName)
        {
            var stringCompare = string.CompareOrdinal(currentUserName, otherUserName) > 0;
            return stringCompare ? $"{currentUserName}-{otherUserName}" : $"{otherUserName}-{currentUserName}";
        }
    }
}
