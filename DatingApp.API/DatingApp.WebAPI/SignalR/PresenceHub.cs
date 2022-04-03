using DatingApp.Business.Services.Authentication;
using Microsoft.AspNetCore.SignalR;
using System.Web.Http;

namespace DatingApp.WebAPI.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly ITokenService _tokenService;
        private readonly PresenceTracker _presenceTracker;

        public PresenceHub(ITokenService tokenService, PresenceTracker presenceTracker)
        {
            _tokenService = tokenService;
            _presenceTracker = presenceTracker;
        }


        public override async Task OnConnectedAsync()
        {
            var userName = _tokenService.GetCurrentUserName(Context.User);

            var userIsOnline = await _presenceTracker.UserConnected(userName, Context.ConnectionId);

            if (userIsOnline)
                await Clients.Others.SendAsync("NotifyForOnlineUser", userName);

            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userName = _tokenService.GetCurrentUserName(Context.User);
            var userIsOffline = await _presenceTracker.UserDisconnected(userName, Context.ConnectionId);

            if (userIsOffline)
                await Clients.Others.SendAsync("NotifyForOfflineUser", userName);

            base.OnDisconnectedAsync(exception);
        }
    }
}
