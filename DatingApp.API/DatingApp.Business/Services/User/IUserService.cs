namespace DatingApp.Business.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterNewUser(UserDto userModel);
        Task<LoggedUserDto> LoginUser(LoginUserDto usermMdel);
        Task<UserDto> UpdateUser(int userId, UserDto userModel);
        Task DeleteUser(int userId);
    }
}
