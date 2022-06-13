using DatingApp.Business.CQRS.User.Commands;
using DatingApp.Business.CQRS.User.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.WebAPI.Controllers
{
    [Route("api/user")]
    public class UserApiController : BaseApiController
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICQRSMediator _mediator;

        public UserApiController(ICurrentUser currentUser, ICQRSMediator mediator)
        {
            _currentUser = currentUser;
            _mediator = mediator;

        }

        /// <summary>
        /// Get info about other member
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("member/{userId}"), HttpGet]
        public MemberDto GetMemberDetails(int userId)
        {
           return _mediator.QueryByParameter<GetUserQuery, MemberDto, int>(userId);
        }

        /// <summary>
        /// Get info about the logged user
        /// </summary>
        /// <returns></returns>
        [Route("user"), HttpGet]
        public UserDto GetUserDetails()
        {
            return _mediator.QueryByParameter<GetUserQuery, UserDto, int>(_currentUser.UserId);
        }

        [Route("user"), HttpPut]
        public async Task<UserDto> UpdateUser(UserDto user)
        {
            return await _mediator.CommandByTwoParameters<UpdateUserCommand, Task<UserDto>, int, UserDto>(_currentUser.UserId, user);
        }

        [Route("user"), HttpDelete]
        public async Task DeleteUser()
        {
            await _mediator.CommandByParameter<DeleteUserCommand, Task, int>(_currentUser.UserId);
        }

        [Route("likeUser/{likedUserId}"), HttpPost]
        public void LikeUser(int likeduserId)
        {
            _mediator.CommandByIdAndTwoParameters<UpdateUserCommand, int, bool>(_currentUser.UserId, likeduserId, true);
        }

        [Route("unlikeUser/{unlikedUserId}"), HttpPost]
        public void UnkikeUser( int unlikedUserId)
        {
            _mediator.CommandByIdAndTwoParameters<UpdateUserCommand, int, bool>(_currentUser.UserId, unlikedUserId, false);
        }

        [Route("likedUsers"), HttpGet]
        public async Task<IEnumerable<UserDto>> GetLikedUsers()
        {
            return await _mediator.QueryByTwoParameters<GetUserQuery, Task<IEnumerable<UserDto>>, int, bool>(_currentUser.UserId, false);
        }

        [Route("likedByUsers"), HttpGet]
        public async Task<IEnumerable<UserDto>> GetLikedByUsers()
        {
            return await _mediator.QueryByTwoParameters<GetUserQuery, Task<IEnumerable<UserDto>>, int, bool>(_currentUser.UserId, true);
        }
    }
}
