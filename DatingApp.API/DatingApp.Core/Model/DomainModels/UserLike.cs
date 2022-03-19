namespace DatingApp.Core.Model
{
    public partial class UserLike
    {
        public UserLike()
        {
        }

        public UserLike(int sourceUserId, int likedUserId)
        {
            SourceUserId = sourceUserId;
            LikedUserId = likedUserId;

            IsDeleted = false;
        }

        public void UnlikeUser()
        {
            IsDeleted = true;
        }
    }
}
