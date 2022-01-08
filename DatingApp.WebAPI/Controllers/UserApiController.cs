using DatingApp.Core.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebAPI.Controllers
{
    public class UserApiController : BaseApiController
    {
        private readonly IUserRepository _userRepo;

        public UserApiController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
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
    }
}
