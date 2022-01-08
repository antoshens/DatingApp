using DatingApp.Core.Model.DomainModels;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace DatingApp.Core.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DataContext Db;
        protected readonly IMapper Mapper;

        public BaseRepository(DataContext db)
        {
            Db = db;
        }

        public BaseRepository(DataContext db, IMapper mapper)
        {
            Db = db;
            Mapper = mapper;
        }

        public virtual async Task<TEntity?> GetByPrimaryKeyAsync(int primarykey)
        {
            return await Db.FindAsync<TEntity>(primarykey);
        }

        public virtual async Task<TEntity?> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Db.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await Db.SaveChangesAsync() > 0;
        }

        public bool SaveAll()
        {
            return Db.SaveChanges() > 0;
        }

        protected virtual IQueryable<TEntity> GetQueryByExpression(Expression<Func<TEntity, bool>> predicate)
        {
            return Db.Set<TEntity>().Where(predicate);
        }
    }
}
