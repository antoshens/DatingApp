namespace DatingApp.Business.CQRS.User.Commands
{
    public class UpdateUserCommand : ICommandHandler,
        ICommandByTwoParametersHandler<Task<UserDto>, int, UserDto>,
        ICommandByIdAndTwoParametersHandler<int, int, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> HandleCommand(int userId, UserDto userModel)
        {
            var userDto = _unitOfWork.UserRepository.UpdateUser(userId, userModel);
            await _unitOfWork.SaveChangesAsync();

            return userDto;
        }

        public int HandleCommand(int sourceUserId, int interactedUserId, bool isLiked)
        {
            var user = _unitOfWork.UserRepository.GetFullUser(sourceUserId);

            if (isLiked)
            {
                user.LikeUser(interactedUserId);
            }
            else
            {
                user.UnlikeUser(interactedUserId);
            }

            var result = _unitOfWork.SaveChanges();

            return result > 0 ? interactedUserId : 0;
        }
    }
}
