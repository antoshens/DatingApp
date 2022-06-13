using System.Linq.Expressions;

namespace DatingApp.Core.Data.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        Task<TEntity?> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate);

        TDestination Map<TSource, TDestination>(TSource source);
    }
}
