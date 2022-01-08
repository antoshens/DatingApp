using DatingApp.Business.Services.Authentication;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Business.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IUserRepository userRepository,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public User GetCurrentUser()
        {
            var userPrincipal = _httpContextAccessor.HttpContext?.User;

            var currentUserId = _tokenService.GetCurrentUserId(userPrincipal);
            var currentUser = _userRepository.GetFullUser(currentUserId.HasValue ? currentUserId.Value : 0);

            return currentUser;
        }

        public int? GetCurrentUserId()
        {
            var userPrincipal = _httpContextAccessor.HttpContext?.User;

            var currentUserId = _tokenService.GetCurrentUserId(userPrincipal);

            return currentUserId;
        }
    }
}
