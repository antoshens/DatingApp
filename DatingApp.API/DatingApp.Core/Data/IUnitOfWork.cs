using DatingApp.Core.Data.Repositories;

namespace DatingApp.Core.Data
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
