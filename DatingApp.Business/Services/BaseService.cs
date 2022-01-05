using DatingApp.Core.Data;

namespace DatingApp.Business.Services
{
    public class BaseService
    {
        protected readonly DataContext db;

        public BaseService(DataContext db)
        {
            this.db = db;
        }
    }
}
