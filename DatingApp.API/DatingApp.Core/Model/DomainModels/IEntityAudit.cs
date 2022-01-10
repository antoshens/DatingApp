namespace DatingApp.Core.Model.DomainModels
{
    public interface IEntityAudit
    {
        int AuditInfoId { get; set; }
        int AuditEntityKey { get; }
        bool IsDeleted { get; set; }
        AuditInfo AuditInfo { get; set; }
    }
}
