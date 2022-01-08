using DatingApp.Business.Services.Authentication;
using DatingApp.Core.Data.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Business.Services
{
    public class UserService : IUserService
    {
        private ITokenService _tokenService;
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<UserDto> RegisterNewUser(UserDto userModel)
        {
            using var hmac = new HMACSHA512();

            var newUser = new User(userModel.UserName,
                hmac.ComputeHash(Encoding.UTF8.GetBytes(userModel.Password)),
                hmac.Key,
                userModel.Email,
                userModel.Interests,
                userModel.LookingFor,
                userModel.City,
                userModel.Country,
                userModel.MainPhotoData,
                userModel.BirthDate,
                userModel.FirstName,
                userModel.LastName,
                userModel.Sex);

            _userRepository.AddUser(newUser);
            await _userRepository.SaveAllAsync();

            return userModel;
        }

        public async Task<LogedUserDto> LoginUser(LoginUserDto usermModel)
        {
            var existedUser = await _userRepository.GetByPredicateAsync(u => u.UserName == usermModel.UserName
                                                    || u.Email == usermModel.UserName);

            if (existedUser == null)
            {
                throw new ArgumentNullException("Login or Password is incorrect");
            }

            using var hmac = new HMACSHA512(existedUser.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(usermModel.Password));

            if (!CompareHashes(computedHash, existedUser.PasswordHash))
            {
                throw new ArgumentNullException("Login or Password is incorrect");
            }

            return new LogedUserDto
            {
                UserName = usermModel.UserName,
                Token = _tokenService.CreateToken(existedUser)
            };
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
    }
}
