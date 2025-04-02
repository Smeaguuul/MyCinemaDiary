using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyCinemaDiary.Infrastructure.Data;

namespace MyCinemaDiary.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class // Enforces reference types. Ingen int, struct osv.
    {
        private readonly AppDBContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new RepositoryException($"{typeof(T).Name} can't be added!", e);
            }

        }
        public async Task RemoveAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new RepositoryException($"{typeof(T).Name} can't be removed!", e);
            }
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            return await GetAllAsync(1, int.MaxValue, predicate, orderBy, include); // Undgår dobbelt op på logik. (Method overloading?)
        }

        public async Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(); // Does not execute before here.
        }
    }
}
