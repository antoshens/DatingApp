namespace DatingApp.Business.CQRS.User.Commands
{
    public class DeleteUserCommand : ICommandHandler,
        ICommandByParameterHandler<Task, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleCommand(int userId)
        {
            var user = _unitOfWork.UserRepository.GetFullUser(userId);

            await _unitOfWork.UserRepository.DeleteUser(user);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
