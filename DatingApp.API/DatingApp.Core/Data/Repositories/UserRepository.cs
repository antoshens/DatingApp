using AutoMapper;
using DatingApp.Core.Model;
using DatingApp.Core.Model.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DatingApp.Core.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager, DataContext db, IMapper mapper) : base(db, mapper)
        {
            _userManager = userManager;
        }

        public T GetUser<T>(int userId)
        {
            var domainUser = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            var userModel = this.Mapper.Map<User, T>(domainUser);

            return userModel;
        }

        public User GetUser(int userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return user;
        }

        public User GetFullUser(int userId)
        {
            var user = GetQueryByExpression(_ => _.Id == userId)
                .Include(x => x.Photos)
                .Include(x => x.LikedUsers)
                .Include(x => x.LikedByUsers)
                .FirstOrDefault();

            return user;
        }

        public override async Task<User?> GetByPredicateAsync(Expression<Func<User, bool>> predicate)
        {
            return await _userManager.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<UserDto>> GetLikedUsers(int sourceUserId)
        {
            var user = await _userManager.Users
                .Include(x => x.LikedUsers.Select(_ => _.LikedUser))
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(u => u.Id == sourceUserId && !u.IsDeleted);

            var likedUsersModel = user?.LikedUsers.Select(x => Mapper.Map<UserDto>(x.LikedUser));

            return likedUsersModel;
        }

        public async Task<IEnumerable<UserDto>> GetLikedByUsers(int sourceUserId)
        {
            var user = await _userManager.Users
                .Include(x => x.LikedByUsers.Select(_ => _.LikedUser))
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(u => u.Id == sourceUserId && !u.IsDeleted);

            var likedByUsersModel = user?.LikedByUsers.Select(x => Mapper.Map<UserDto>(x.SourceUser));

            return likedByUsersModel;
        }

        public UserDto UpdateUser(int userId, UserDto userModel)
        {
            var user = GetUser(userId);
            user.UpdateMainUserFields(userModel.Interests,
               userModel.LookingFor,
               userModel.City,
               userModel.Country,
               userModel.BirthDate,
               userModel.FirstName,
               userModel.LastName,
               userModel.Sex);

            _userManager.UpdateAsync(user);

            return userModel;
        }

        public async Task<UserDto> AddUser(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) return null;

            var userDto = this.Mapper.Map<User, UserDto>(user);

            return userDto;
        }

        public async Task DeleteUser(User user)
        {
            await _userManager.DeleteAsync(user);
        }

        public UserLike LikeUser(User user, int likedUserId)
        {
            var likedUser = user.LikeUser(likedUserId);

            Db.SaveChanges();

            return likedUser;
        }

        public void UnlikeUser(User user, int likedUserId)
        {
            user.UnlikeUser(likedUserId);

            Db.SaveChanges();
        }
    }
}
