using DatingApp.Business.Services.Authentication;
using Microsoft.AspNetCore.SignalR;
using System.Web.Http;

namespace DatingApp.WebAPI.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly ITokenService _tokenService;

        public PresenceHub(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }


        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync("NotifyForOnlineUser", _tokenService.GetCurrentUserName(Context.User));
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync("NotifyForOfflineUser", _tokenService.GetCurrentUserName(Context.User));

            base.OnDisconnectedAsync(exception);
        }
    }
}
