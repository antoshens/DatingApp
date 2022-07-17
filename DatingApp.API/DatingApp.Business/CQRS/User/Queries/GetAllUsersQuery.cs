using DatingApp.Business.Model;

namespace DatingApp.Business.CQRS.User.Queries
{
    public class GetAllUsersQuery : IQueryHandler,
        IQueryByTwoParametersHandler<IEnumerable<MemberDto>, ODataParameters, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllUsersQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MemberDto> HandleQuery(ODataParameters filter, int excludeId)
        {
            var allUsersDto = _unitOfWork.UserRepository.GetAllUsers<MemberDto>(filter.Skip, filter.Take);
            var allOtherUsersDto = allUsersDto.Where(u => u.Id != excludeId);

            return allOtherUsersDto;
        }
    }
}
