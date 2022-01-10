namespace DatingApp.Core.Model
{
    public class AuditInfo
    {
        public int AuditInfoId { get; set; }
        public int PrimaryKey { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
