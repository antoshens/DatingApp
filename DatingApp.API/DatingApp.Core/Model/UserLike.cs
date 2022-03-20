namespace DatingApp.Core.Model
{
    public partial class UserLike
    {
        public User SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public User LikedUser { get; set; }
        public int LikedUserId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
