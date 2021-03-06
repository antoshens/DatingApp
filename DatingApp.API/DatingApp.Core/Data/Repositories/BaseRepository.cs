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

        public virtual async Task<TEntity?> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Db.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        protected virtual IQueryable<TEntity> GetQueryByExpression(Expression<Func<TEntity, bool>> predicate)
        {
            return Db.Set<TEntity>().Where(predicate);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}
