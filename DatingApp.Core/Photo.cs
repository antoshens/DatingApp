using DatingApp.Core.Model;
using DatingApp.Core.Model.DomainModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Core
{
    public partial class Photo : IEntity, IEntityAudit
    {
        [NotMapped]
        public int PrimaryKey => PhotoId;
        public int AuditInfoId { get; set; }
        [NotMapped]
        public int AuditEntityKey => PhotoId;
        public bool IsDeleted { get; set; } = false;

        public int PhotoId { get; private set; }
        public bool IsMain { get; private set; }
        public byte[] PhotoData { get; private set; }
        public int UserId { get; private set; }


        // Foreign Keys (FK)
        public AuditInfo AuditInfo { get; set; } // 1:1 (FK) AuditInfo.AuditInfoId
        public User User { get; private set; } // M:1 (FK) User.UserId
    }
}
