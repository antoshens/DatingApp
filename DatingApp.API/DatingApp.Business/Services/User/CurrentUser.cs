using DatingApp.Business.Services.Authentication;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Business.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public User GetCurrentUser()
        {
            var userPrincipal = _httpContextAccessor.HttpContext?.User;

            var currentUserId = _tokenService.GetCurrentUserId(userPrincipal);
            var currentUser = _unitOfWork.UserRepository.GetUser(currentUserId.HasValue ? currentUserId.Value : 0);

            return currentUser;
        }

        public int UserId
        {
            get
            {
                var userPrincipal = _httpContextAccessor.HttpContext?.User;

                if (userPrincipal is null) return 0;

                var currentUserId = _tokenService.GetCurrentUserId(userPrincipal);

                return currentUserId ?? 0;
            }
        }
    }
}
