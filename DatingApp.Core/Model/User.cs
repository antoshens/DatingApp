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

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte? Sex { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Interests { get; set; }
        public string LookingFor {get; set;}
        public string City { get; set; }
        public string Country { get; set; }

        // Foreign Keys (FK)
        public AuditInfo AuditInfo { get; set; } // 1:1 (FK) AuditInfo.AuditInfoId
        public ICollection<Photo> Photos { get; set; } // 1:M (FK) Photo.PhotoId
    }
}
