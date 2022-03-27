using DatingApp.Business.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace DatingApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;

        public UserService(IUserRepository userRepository, SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        public async Task<UserDto> RegisterNewUser(UserDto userModel)
        {
            using var hmac = new HMACSHA512();

            var mainPhotoDto = userModel.Photos.FirstOrDefault(p => p.IsMain);

            var newUser = new User(userModel.UserName,
                userModel.Email,
                userModel.Interests,
                userModel.LookingFor,
                userModel.City,
                userModel.Country,
                mainPhotoDto is null,
                mainPhotoDto?.PublicId ?? "",
                mainPhotoDto?.Url ?? "",
                userModel.BirthDate,
                userModel.FirstName,
                userModel.LastName,
                userModel.Sex);

            var result = await _userRepository.AddUser(newUser, userModel.Password);

            return result;
        }

        public async Task<LoggedUserDto> LoginUser(LoginUserDto userModel)
        {
            var existedUser = await _userRepository.GetByPredicateAsync(u => u.UserName == userModel.UserName
                                                    || u.Email == userModel.UserName);

            if (existedUser == null)
            {
                throw new ArgumentNullException("Login or Password is incorrect");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(existedUser, userModel.Password, false);

            if (!result.Succeeded) return null;

            return new LoggedUserDto
            {
                UserName = userModel.UserName
            };
        }

        public async Task<bool> LogoutUser(int userId)
        {
            var existedUser = await _userRepository.GetByPredicateAsync(u => u.Id == userId);

            if (existedUser == null)
            {
                return false;
            }

            return true;
        }

        public async Task<UserDto> UpdateUser(int userId, UserDto userModel)
        {
            var userDto = _userRepository.UpdateUser(userId, userModel);
            await _userRepository.SaveAllAsync();

            return userDto;
        }

        public async Task DeleteUser(int userId)
        {
            var user = _userRepository.GetFullUser(userId);

            _userRepository.DeleteUser(user);

            await _userRepository.SaveAllAsync();
        }

        public UserLike LikeUser(int sourceUserId, int likedUserId)
        {
            var user = _userRepository.GetFullUser(sourceUserId);

            return _userRepository.LikeUser(user, likedUserId);
        }

        public void UnlikeUser(int sourceUserId, int inlikedUserId)
        {
            var user = _userRepository.GetFullUser(sourceUserId);

            _userRepository.UnlikeUser(user, inlikedUserId);
        }

        public Task<IEnumerable<UserDto>> GetLikedUsers(int userId)
        {
            var likedUsers = _userRepository.GetLikedUsers(userId);

            return likedUsers;
        }

        public Task<IEnumerable<UserDto>> GetLikedByUsers(int userId)
        {
            var likedByUsers = _userRepository.GetLikedByUsers(userId);

            return likedByUsers;
        }
    }
}
