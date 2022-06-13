using DatingApp.Business.Services.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Business.CQRS.User.Commands
{
    public class AuthUserCommand : ICommandHandler,
        ICommandByParameterHandler<Task<UserDto>, UserDto>,
        ICommandByParameterHandler<Task<LoggedUserDto>, LoginUserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<Core.Model.User> _signInManager;

        public AuthUserCommand(IUnitOfWork unitOfWork, ITokenService tokenService, SignInManager<Core.Model.User> signInManager)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<UserDto> HandleCommand(UserDto userModel)
        {
            var mainPhotoDto = userModel.Photos.FirstOrDefault(p => p.IsMain);

            var publicPhotoId = mainPhotoDto?.PublicId ?? String.Empty;
            var photoUrl = mainPhotoDto?.PublicId ?? String.Empty;

            var newUser = new Core.Model.User(
                userModel.UserName,
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
                var result = await _unitOfWork.UserRepository.AddUser(newUser, userModel.Password);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoggedUserDto> HandleCommand(LoginUserDto userModel)
        {
            var existedUser = await _unitOfWork.UserRepository.GetByPredicateAsync(u => u.UserName == userModel.UserName
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
    }
}
