using DatingApp.Core.Model.DomainModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Core.Model
{
    public partial class User : IdentityUser<int>, IEntity, IEntityAudit
    {
        [NotMapped]
        public int PrimaryKey => Id;
        [NotMapped]
        public int AuditEntityKey => Id;
        public bool IsDeleted { get; set; }
        public int AuditInfoId { get; set; }

        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public byte? Sex { get; private set; }
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
        public ICollection<Message> SentMessages { get; set; } // 1:M (FK) Message.MessageId
        public ICollection<Message> ReceivedMessages { get; set; } // M:1 (FK) Message.MessageId
        public ICollection<UserRole> UserRoles { get; set; } // M:1 (FK) UserRole.RoleId
    }
}
