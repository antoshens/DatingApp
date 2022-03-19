using AutoMapper;
using DatingApp.Core.Model;
using DatingApp.Core.Model.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Core.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DataContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public T GetUser<T>(int userId)
        {
            var domainUser = GetUser(userId);

            var userModel = this.Mapper.Map<User, T>(domainUser);

            return userModel;
        }

        public User GetUser(int userId)
        {
            var user = Db.Users.SingleOrDefault(u => u.UserId == userId);

            return user;
        }

        public User GetFullUser(int userId)
        {
            var user = GetQueryByExpression(_ => _.UserId == userId)
                .Include(x => x.Photos)
                .Include(x => x.LikedUsers)
                .Include(x => x.LikedByUsers)
                .FirstOrDefault();

            return user;
        }

        public async Task<IEnumerable<UserDto>> GetLikedUsers(int sourceUserId)
        {
            var user = await Db.Users
                .Include(x => x.LikedUsers.Select(_ => _.LikedUser))
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(u => u.UserId == sourceUserId && !u.IsDeleted);

            var likedUsersModel = user?.LikedUsers.Select(x => Mapper.Map<UserDto>(x.LikedUser));

            return likedUsersModel;
        }

        public UserDto UpdatUser(User user)
        {
            Db.Entry(user).State = EntityState.Modified;

            var userDto = this.Mapper.Map<User, UserDto>(user);

            return userDto;
        }

        public UserDto AddUser(User user)
        {
            Db.Users.Add(user);

            var userDto = this.Mapper.Map<User, UserDto>(user);

            return userDto;
        }

        public void DeleteUser(User user)
        {
            Db.Users.Remove(user);
        }
    }
}
