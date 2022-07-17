using DatingApp.Core.Model;
using DatingApp.Core.Model.DTOs;
using System.Linq.Expressions;

namespace DatingApp.Core.Data.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        IEnumerable<T> GetAllUsers<T>(int skip, int take);
        T GetUser<T>(int userId);
        User GetUser(int userId);
        IQueryable<User> GetUser(int userId, Expression<Func<User, bool>>? predicate);
        User GetFullUser(int userId);
        UserDto UpdateUser(int userId, UserDto userModel);
        Task<UserDto> AddUser(User user, string password);
        Task DeleteUser(User user);
    }
}
