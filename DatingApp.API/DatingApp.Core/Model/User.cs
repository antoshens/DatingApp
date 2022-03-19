using DatingApp.Core.Model.DomainModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Core.Model
{
    public partial class User : IEntity, IEntityAudit
    {
        [NotMapped]
        public int PrimaryKey => UserId;
        [NotMapped]
        public int AuditEntityKey => UserId;
        public bool IsDeleted { get; set; }
        public int AuditInfoId { get; set; }

        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public byte? Sex { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public DateTime LastActive { get; private set; } = DateTime.UtcNow;
        public string Interests { get; private set; }
        public string LookingFor {get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }

        // Foreign Keys (FK)
        public AuditInfo AuditInfo { get; set; } // 1:1 (FK) AuditInfo.AuditInfoId
        public ICollection<Photo> Photos { get; private set; } // 1:M (FK) Photo.PhotoId
        public ICollection<UserLike> LikedByUsers { get; set; } // M:1 (FK) UserLike.UserLikeId
        public ICollection<UserLike> LikedUsers { get; set; } // 1:M (FK) UserLike.UserLikeId
    }
}
