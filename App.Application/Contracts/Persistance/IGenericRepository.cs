using System.Linq.Expressions;

namespace App.Application.Contracts.Persistance
{
    public interface IGenericRepository<T, TId> where T : class where TId : struct
    {
        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllPagedAsync(int pageNumber, int pageSize);

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        
        ValueTask<T?> GetByIdAsync(int id);

        ValueTask AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        ValueTask<bool> AnyAsync(TId id);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
