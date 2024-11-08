using System.Linq.Expressions;

namespace DataLayer.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        Task<int> CountAsync();
        Task<IEnumerable<T>> GetAllAsyncByBoolFilter(Expression<Func<T, bool>> filter);
        IQueryable<T> GetAllAsQueryable();
        Task<IEnumerable<T>> GetAllIncludingAsync(
            Expression<Func<T, bool>> filter = null,
            params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
    }
}
