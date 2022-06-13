using DatingApp.Business.CQRS.User.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    public class AccountApiController : BaseApiController
    {
        private readonly ICQRSMediator _mediator;

        public AccountApiController(ICQRSMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<UserDto> Register(UserDto model)
        {
            var result = await _mediator.CommandByParameter<AuthUserCommand, Task<UserDto>, UserDto>(model);

            return result;
        }

        [HttpPost("login")]
        public async Task<LoggedUserDto> Login(LoginUserDto model)
        {
            var result = await _mediator.CommandByParameter<AuthUserCommand, Task<LoggedUserDto>, LoginUserDto>(model);

            return result;
        }
    }
}
