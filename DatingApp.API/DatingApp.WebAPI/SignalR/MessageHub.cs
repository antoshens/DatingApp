using DatingApp.Business.Services.Authentication;
using DatingApp.Business.Services.Message;
using DatingApp.Core.Model;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.WebAPI.SignalR
{
    public class MessageHub : Hub
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _presenceTracker;

        public MessageHub(ITokenService tokenService, IMessageService messageService,
             IUserService userService, IHubContext<PresenceHub> presenceHub, PresenceTracker presenceTracker)
        {
            _tokenService = tokenService;
            _messageService = messageService;
            _userService = userService;
            _presenceHub = presenceHub;
            _presenceTracker = presenceTracker;
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

        public async Task<Group> AddToGroup(string groupName)
        {
            var group = await _messageService.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, _tokenService.GetCurrentUserName(Context.User));

            if (group is null)
            {
                group = new Group(groupName);
                _messageService.AddGroup(group);
            }

            await _messageService.AddConnectionToGroup(groupName, connection);

            return group;
        }

        public async Task RemoveFromGroup()
        {
            var connection = await _messageService.GetConnection(Context.ConnectionId);
            _messageService.RemoveConnection(connection);
        }

        public async Task SendMessage(SendMessageDto message)
        {
            var httpContext = Context.GetHttpContext();
            var otherUserName = httpContext.Request.Query["user"].ToString();

            var currentUserName = _tokenService.GetCurrentUserName(Context.User);
            var currentUser = await _userService.GetUserByName(currentUserName);

            var groupName = GetGroupName(currentUserName, otherUserName);
            var group = await _messageService.GetMessageGroup(groupName);

            if (!group.Connections.Any(c => c.UserName == otherUserName))
            {
                var connections = await _presenceTracker.GetConnectionsForUser(otherUserName);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", new
                    {
                        firstname = currentUser.FirstName,
                        lastname = currentUser.LastName,
                        username = currentUserName
                    });
                }
            }

            try
            {
                var respose = _messageService.CreateMessage(currentUser.Id, message);

                await Clients.Group(groupName).SendAsync("NewMessage", respose);
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }
        }

        private string GetGroupName(string currentUserName, string otherUserName)
        {
            var stringCompare = string.CompareOrdinal(currentUserName, otherUserName) > 0;
            return stringCompare ? $"{currentUserName}-{otherUserName}" : $"{otherUserName}-{currentUserName}";
        }
    }
}
