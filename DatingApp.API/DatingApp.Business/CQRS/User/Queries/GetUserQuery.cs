using Microsoft.EntityFrameworkCore;

namespace DatingApp.Business.CQRS.User.Queries
{
    public class GetUserQuery : IQueryHandler,
        IQueryByParameterHandler<UserDto, int>,
        IQueryByParameterHandler<MemberDto, int>,
        IQueryByTwoParametersHandler<Task<IEnumerable<UserDto>>, int, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserDto HandleQuery(int userId)
        {
            return _unitOfWork.UserRepository.GetUser<UserDto>(userId);
        }

        public async Task<IEnumerable<UserDto>> HandleQuery(int userId, bool isLikedBy)
        {
            Core.Model.User? user;

            if (isLikedBy)
            {
                user = await _unitOfWork.UserRepository.GetUser(userId, _ => !_.IsDeleted)
                    .Include(x => x.LikedUsers.Select(_ => _.LikedUser))
                    .Include(x => x.Photos)
                    .FirstOrDefaultAsync();

                if (user is null)
                {
                    throw new Exception("Couldn't find a user");
                }

                var likedUsersModel = user.LikedUsers.Select(x => _unitOfWork.UserRepository.Map<Core.Model.User, UserDto>(x.LikedUser));

                return likedUsersModel;
            }

            user = await _unitOfWork.UserRepository.GetUser(userId, _ => !_.IsDeleted)
                    .Include(x => x.LikedByUsers.Select(_ => _.LikedUser))
                    .Include(x => x.Photos)
                    .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new Exception("Couldn't find a user");
            }

            var likedByUsersModel = user.LikedByUsers.Select(x => _unitOfWork.UserRepository.Map<Core.Model.User, UserDto>(x.SourceUser));

            return likedByUsersModel;
        }

        MemberDto IQueryByParameterHandler<MemberDto, int>.HandleQuery(int userId)
        {
            return _unitOfWork.UserRepository.GetUser<MemberDto>(userId);
        }
    }
}
