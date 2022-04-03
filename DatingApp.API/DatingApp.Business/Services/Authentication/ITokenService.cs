using System.Security.Claims;

namespace DatingApp.Business.Services.Authentication
{
    public interface ITokenService
    {
        string CreateToken(User user);
        int? GetCurrentUserId(ClaimsPrincipal user);
        string GetCurrentUserName(ClaimsPrincipal principal);
    }
}
