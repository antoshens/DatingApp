using DatingApp.Business.Services.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private ITokenService _tokenService;

        public UserService(DataContext db, ITokenService tokenService) : base(db)
        {
            _tokenService = tokenService;
        }

        public async Task<RegisterUserDto> RegisterNewUser(RegisterUserDto userModel)
        {
            using var hmac = new HMACSHA512();

            var newUser = new User(userModel.UserName,
                hmac.ComputeHash(Encoding.UTF8.GetBytes(userModel.Password)),
                hmac.Key,
                userModel.Email,
                userModel.BirthDate,
                userModel.FirstName,
                userModel.LastName,
                userModel.Sex);

            db.Users.Add(newUser);
            await db.SaveChangesAsync();

            return userModel;
        }

        public async Task<LogedUserDto> LoginUser(LoginUserDto usermModel)
        {
            var existedUser = await db.Users.SingleOrDefaultAsync(u => u.UserName == usermModel.UserName
                                                    || u.Email == usermModel.UserName);

            if (existedUser == null)
            {
                throw new ArgumentNullException("Login or Password is incorrect");
            }

            using var hmac = new HMACSHA512(existedUser.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(usermModel.Password));

            for (int i = 0; i < computedHash.Length; ++i)
            {
                if (computedHash[i] != existedUser.PasswordHash[i])
                {
                    throw new ArgumentNullException("Login or Password is incorrect");
                }
            }

            return new LogedUserDto
            {
                UserName = usermModel.UserName,
                Token = _tokenService.CreateToken(existedUser)
            };
        }
    }
}
