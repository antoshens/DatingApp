using DatingApp.Core.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebAPI.Controllers
{
    public class UserApiController : BaseApiController
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;

        public UserApiController(IUserRepository userRepo, IUserService userService)
        {
            _userRepo = userRepo;
            _userService = userService;

        }

        [Route("member/{userId}"), HttpGet]
        public MemberDto GetMemberDetails(int userId)
        {
           return _userRepo.GetUser<MemberDto>(userId);
        }

        [Route("user/{userId}"), HttpGet]
        public UserDto GetUserDetails(int userId)
        {
            return _userRepo.GetUser<UserDto>(userId);
        }

        [Route("user/{userId}"), HttpPut]
        public async Task<UserDto> UpdateUser(int userId, UserDto user)
        {
            return await _userService.UpdateUser(userId, user);
        }

        [Route("user/{userId}"), HttpDelete]
        public void DeleteUser(int userId)
        {
            _userService.DeleteUser(userId);
        }
    }
}
