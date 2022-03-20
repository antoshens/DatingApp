namespace DatingApp.Business.Services
{
    public interface ICurrentUser
    {
        int UserId { get; }
        User GetCurrentUser();
    }
}
