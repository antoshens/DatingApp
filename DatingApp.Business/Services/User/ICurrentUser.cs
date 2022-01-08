namespace DatingApp.Business.Services
{
    public interface ICurrentUser
    {
        int? GetCurrentUserId();
        User GetCurrentUser();
    }
}
