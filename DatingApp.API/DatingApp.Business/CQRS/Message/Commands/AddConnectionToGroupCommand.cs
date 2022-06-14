namespace DatingApp.Business.CQRS.Message.Commands
{
    public class AddConnectionToGroupCommand : ICommandHandler,
        ICommandByTwoParametersHandler<Task, string, Connection>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddConnectionToGroupCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleCommand(string groupName, Connection connection)
        {
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);

            group.Connections.Add(connection);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
