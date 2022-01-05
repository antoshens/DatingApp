namespace DatingApp.Core.Model.DomainModels
{
    public interface IEntity
    {
        public int PrimaryKey { get; }
        public bool IsDeleted { get; set; }
    }
}
