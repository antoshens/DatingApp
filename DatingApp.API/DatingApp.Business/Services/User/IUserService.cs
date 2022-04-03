namespace DatingApp.Business.Services
{
    public interface IUserService
    {
        Task<BusinessResponse<UserDto>> RegisterNewUser(UserDto userModel);
        Task<BusinessResponse<LoggedUserDto>> LoginUser(LoginUserDto usermMdel);
        Task<bool> LogoutUser(int userId);
        Task<UserDto> UpdateUser(int userId, UserDto userModel);
        Task<User> GetUserByName(string username);
        Task DeleteUser(int userId);
        UserLike LikeUser(int sourceUserId, int likedUserId);
        void UnlikeUser(int sourceUserId, int inlikedUserId);
        Task<IEnumerable<UserDto>> GetLikedUsers(int userId);
        Task<IEnumerable<UserDto>> GetLikedByUsers(int userId);
    }
}
