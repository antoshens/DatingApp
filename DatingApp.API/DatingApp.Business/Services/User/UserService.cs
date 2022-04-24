using DatingApp.Business.Services.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        public UserService(IUserRepository userRepository, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<UserDto> RegisterNewUser(UserDto userModel)
        {
            var mainPhotoDto = userModel.Photos.FirstOrDefault(p => p.IsMain);

            var publicPhotoId = mainPhotoDto?.PublicId ?? "";
            var photoUrl = mainPhotoDto?.PublicId ?? "";

            var newUser = new User(userModel.UserName,
                userModel.Email,
                userModel.Interests,
                userModel.LookingFor,
                userModel.City,
                userModel.Country,
                publicPhotoId,
                photoUrl,
                userModel.BirthDate,
                userModel.FirstName,
                userModel.LastName,
                userModel.Sex);

            try
            {
                var result = await _userRepository.AddUser(newUser, userModel.Password);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoggedUserDto> LoginUser(LoginUserDto userModel)
        {
            var existedUser = await _userRepository.GetByPredicateAsync(u => u.UserName == userModel.UserName
                                                    || u.Email == userModel.UserName);

            if (existedUser == null)
            {
                throw new ArgumentException("Login or Password is incorrect");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(existedUser, userModel.Password, false);

            if (!result.Succeeded)
            {
                throw new ArgumentException("Login or Password is incorrect");
            }

            return new LoggedUserDto
            {
                UserName = userModel.UserName,
                Token = _tokenService.CreateToken(existedUser)
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

        public async Task<User> GetUserByName(string username)
        {
            var user = await _userRepository.GetByPredicateAsync(u => u.UserName == username && !u.IsDeleted);

            if (user is null) throw new ArgumentNullException(nameof(user));

            return user;
        }

        public async Task DeleteUser(int userId)
        {
            var user = _userRepository.GetFullUser(userId);

            await _userRepository.DeleteUser(user);

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
