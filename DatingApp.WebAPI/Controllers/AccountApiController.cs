using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebAPI.Controllers
{
    [AllowAnonymous]
    public class AccountApiController : BaseApiController
    {
        private readonly IUserService _userService;

        public AccountApiController(IUserService userService) {
            this._userService = userService;
        }

        [HttpPost("register")]
        public async Task<UserDto> Register(UserDto model)
        {
            return await _userService.RegisterNewUser(model);
        }

        [HttpPost("login")]
        public async Task<LogedUserDto> Login(LoginUserDto model)
        {
            return await _userService.LoginUser(model);
        }
    }
}
