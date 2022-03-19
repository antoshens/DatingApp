using DatingApp.Core.Model;
using DatingApp.Core.Model.DTOs;

namespace DatingApp.Core.Data.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        T GetUser<T>(int userId);
        User GetUser(int userId);
        User GetFullUser(int userId);
        UserDto UpdatUser(User user);
        UserDto AddUser(User user);
        void DeleteUser(User user);
        Task<IEnumerable<UserDto>> GetLikedUsers(int sourceUserId);
    }
}
