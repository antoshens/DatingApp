using DatingApp.Business.Services.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<UserDto> RegisterNewUser(UserDto userModel)
        {
            using var hmac = new HMACSHA512();

            var mainPhotoDto = userModel.Photos.First(p => p.IsMain);

            var newUser = new User(userModel.UserName,
                userModel.Email,
                userModel.Interests,
                userModel.LookingFor,
                userModel.City,
                userModel.Country,
                mainPhotoDto.PublicId,
                mainPhotoDto.Url,
                userModel.BirthDate,
                userModel.FirstName,
                userModel.LastName,
                userModel.Sex);

            _userRepository.AddUser(newUser);
            await _userRepository.SaveAllAsync();

            return userModel;
        }

        public async Task<LoggedUserDto> LoginUser(LoginUserDto usermModel)
        {
            var existedUser = await _userRepository.GetByPredicateAsync(u => u.UserName == usermModel.UserName
                                                    || u.Email == usermModel.UserName);

            if (existedUser == null)
            {
                throw new ArgumentNullException("Login or Password is incorrect");
            }

            return new LoggedUserDto
            {
                UserName = usermModel.UserName,
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

        private bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
            {
                return false;
            }

            for (int i = 0; i < hash1.Length; ++i)
            {
                if (hash1[i] != hash2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<UserDto> UpdateUser(int userId, UserDto userModel)
        {
            var user = _userRepository.GetUser(userId);

            user.UpdateMainUserFields(userModel.Interests,
                userModel.LookingFor,
                userModel.City,
                userModel.Country,
                userModel.BirthDate,
                userModel.FirstName,
                userModel.LastName,
                userModel.Sex);

            var userDto = _userRepository.UpdatUser(user);
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
