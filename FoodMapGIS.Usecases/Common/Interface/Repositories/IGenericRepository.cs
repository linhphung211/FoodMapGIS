using System.Linq.Expressions;

namespace FoodMapGIS.Application.Common.Interface.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // Truy vấn đọc (Query)
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

        // Thao tác ghi (Command)
        Task CreateAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void DeleteRange(IEnumerable<T> entities);
        void Delete(T entity);
    }
}