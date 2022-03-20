using DatingApp.Core.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebAPI.Controllers
{
    [Route("api/user")]
    public class UserApiController : BaseApiController
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;

        public UserApiController(IUserRepository userRepo, IUserService userService, ICurrentUser currentUser)
        {
            _userRepo = userRepo;
            _userService = userService;
            _currentUser = currentUser;

        }

        /// <summary>
        /// Get info about other member
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("member/{userId}"), HttpGet]
        public MemberDto GetMemberDetails(int userId)
        {
           return _userRepo.GetUser<MemberDto>(userId);
        }

        /// <summary>
        /// Get info about the logged user
        /// </summary>
        /// <returns></returns>
        [Route("user"), HttpGet]
        public UserDto GetUserDetails()
        {
            return _userRepo.GetUser<UserDto>(_currentUser.UserId);
        }

        [Route("user"), HttpPut]
        public async Task<UserDto> UpdateUser(UserDto user)
        {
            return await _userService.UpdateUser(_currentUser.UserId, user);
        }

        [Route("user"), HttpDelete]
        public void DeleteUser()
        {
            _userService.DeleteUser(_currentUser.UserId);
        }

        [Route("likeUser/{likedUserId}"), HttpPost]
        public void LikeUser(int likeduserId)
        {
            _userService.LikeUser(_currentUser.UserId, likeduserId);
        }

        [Route("unlikeUser/{unlikedUserId}"), HttpPost]
        public void UnkikeUser( int unlikedUserId)
        {
            _userService.UnlikeUser(_currentUser.UserId, unlikedUserId);
        }

        [Route("likedUsers"), HttpGet]
        public async Task<IEnumerable<UserDto>> GetLikedUsers()
        {
            return await _userService.GetLikedUsers(_currentUser.UserId);
        }

        [Route("likedByUsers"), HttpGet]
        public async Task<IEnumerable<UserDto>> GetLikedByUsers()
        {
            return await _userService.GetLikedByUsers(_currentUser.UserId);
        }
    }
}
