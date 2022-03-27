using DatingApp.Core.Model;
using DatingApp.Core.Model.DTOs;

namespace DatingApp.Core.Data.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        T GetUser<T>(int userId);
        User GetUser(int userId);
        User GetFullUser(int userId);
        UserDto UpdateUser(int userId, UserDto userModel);
        Task<UserDto> AddUser(User user, string password);
        Task DeleteUser(User user);
        Task<IEnumerable<UserDto>> GetLikedUsers(int sourceUserId);
        Task<IEnumerable<UserDto>> GetLikedByUsers(int sourceUserId);
        UserLike LikeUser(User user, int likedUserId);
        void UnlikeUser(User user, int likedUserId);
    }
}
