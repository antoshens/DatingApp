namespace DatingApp.Business.CQRS.User.Commands
{
    public class DeleteUserCommand : ICommandHandler,
        ICommandByParameterHandler<Task<int>, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> HandleCommand(int userId)
        {
            var user = _unitOfWork.UserRepository.GetFullUser(userId);

            await _unitOfWork.UserRepository.DeleteUser(user);

            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0 ? userId : 0;
        }
    }
}
