namespace DatingApp.Business.Services.Authentication
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
