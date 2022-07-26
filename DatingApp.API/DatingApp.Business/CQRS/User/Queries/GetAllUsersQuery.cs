using DatingApp.Business.Model;
using Microsoft.EntityFrameworkCore;

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

            var currentUser = _unitOfWork.UserRepository.GetUser(excludeId, _ => !_.IsDeleted)
                    .Include(x => x.LikedUsers)
                    .FirstOrDefault();

            if (currentUser is null)
            {
                throw new Exception("Couldn't find a user");
            }

            var likedUsersIds = currentUser.LikedUsers.Select(x => x.LikedUserId);
            var allNotLikedUsers = allUsersDto.Where(u => likedUsersIds.All(id => id != u.Id));
            var allOtherUsersDto = allNotLikedUsers.Where(u => u.Id != excludeId);

            return allOtherUsersDto;
        }
    }
}
