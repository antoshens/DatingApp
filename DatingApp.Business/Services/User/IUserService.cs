namespace DatingApp.Business.Services
{
    public interface IUserService
    {
        Task<RegisterUserDto> RegisterNewUser(RegisterUserDto userModel);
        Task<LogedUserDto> LoginUser(LoginUserDto usermMdel);
    }
}
