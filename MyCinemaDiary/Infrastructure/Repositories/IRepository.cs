using System.Linq.Expressions;

namespace MyCinemaDiary.Infrastructure.Repositories
{
    public interface IRepository<T>
    {
        Task AddAsync(T entity);
        Task RemoveAsync(T entity);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null); 
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? include = null);
    }
}
