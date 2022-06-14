using DatingApp.Business.CQRS.Message.Commands;
using DatingApp.Business.Services.Authentication;
using DatingApp.Core.Data;
using DatingApp.Core.Model;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.WebAPI.SignalR
{
    public class MessageHub : Hub
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICQRSMediator _mediator;
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _presenceTracker;

        public MessageHub(ITokenService tokenService, IUnitOfWork unitOfWork, ICQRSMediator mediator,
            IHubContext<PresenceHub> presenceHub, PresenceTracker presenceTracker)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _presenceHub = presenceHub;
            _presenceTracker = presenceTracker;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUserName = httpContext.Request.Query["user"].ToString();
            var currentUserName = _tokenService.GetCurrentUserName(Context.User);

            var groupName = GetGroupName(currentUserName, otherUserName);

            var currentUser = await _unitOfWork.UserRepository.GetByPredicateAsync(u => u.UserName == currentUserName && !u.IsDeleted);
            var otherUser = await _unitOfWork.UserRepository.GetByPredicateAsync(u => u.UserName == otherUserName && !u.IsDeleted);

            var messages = await _unitOfWork.MessageRepository.GetMessageThread(otherUser.Id, currentUser.Id);

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<Group> AddToGroup(string groupName)
        {
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, _tokenService.GetCurrentUserName(Context.User));

            if (group is null)
            {
                group = new Group(groupName);
                _unitOfWork.MessageRepository.AddGroup(group);
            }

            await _mediator.CommandByTwoParameters<AddConnectionToGroupCommand, Task, string, Connection>(groupName, connection);

            return group;
        }

        public async Task RemoveFromGroup()
        {
            var connection = await _unitOfWork.MessageRepository.GetConnection(Context.ConnectionId);
            _unitOfWork.MessageRepository.RemoveConnection(connection);

            _unitOfWork.SaveChanges();
        }

        public async Task SendMessage(SendMessageDto message)
        {
            var httpContext = Context.GetHttpContext();
            var otherUserName = httpContext.Request.Query["user"].ToString();

            var currentUserName = _tokenService.GetCurrentUserName(Context.User);
            var currentUser = await _unitOfWork.UserRepository.GetByPredicateAsync(u => u.UserName == currentUserName && !u.IsDeleted);

            var groupName = GetGroupName(currentUserName, otherUserName);
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);

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
                var respose = _mediator.CommandByTwoParameters<CreateNewMessageCommand, Message, int, SendMessageDto>(currentUser.Id, message);

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
