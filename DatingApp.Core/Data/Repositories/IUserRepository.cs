using DatingApp.Core.Model;
using DatingApp.Core.Model.DTOs;

namespace DatingApp.Core.Data.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        T GetUser<T>(int userId);
        User GetFullUser(int userId);
        User UpdatUser(UserDto user);
        UserDto AddUser(User user);
    }
}
