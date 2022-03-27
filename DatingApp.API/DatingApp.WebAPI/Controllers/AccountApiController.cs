using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    public class AccountApiController : BaseApiController
    {
        private readonly IUserService _userService;

        public AccountApiController(IUserService userService) {
            this._userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserDto model)
        {
            var result = await _userService.RegisterNewUser(model);

            return result is null ? BadRequest() : Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoggedUserDto>> Login(LoginUserDto model)
        {
            var result = await _userService.LoginUser(model);

            return result is null ? Unauthorized() : Ok(result);
        }
    }
}
