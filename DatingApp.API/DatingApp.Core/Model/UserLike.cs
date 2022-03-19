namespace DatingApp.Core.Model
{
    public partial class UserLike
    {
        public int UserLikeId { get; set; }
        public User SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public User LikedUser { get; set; }
        public int LikedUserId { get; set; }

        public bool IsDeleted { get; set; }
        public int AuditInfoId { get; set; }

        // Foreign Keys (FK)
        public AuditInfo AuditInfo { get; set; } // 1:1 (FK) AuditInfo.AuditInfoId
    }
}
