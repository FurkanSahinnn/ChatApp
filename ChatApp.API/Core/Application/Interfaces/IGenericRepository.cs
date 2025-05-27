using System.Linq.Expressions;

namespace ChatApp.API.Core.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class, new()
    {

        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(object id); // int id
        Task<T?> WhereAsync(Expression<Func<T, bool>> predicate);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
