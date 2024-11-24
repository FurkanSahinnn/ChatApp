using ChatApp.API.Core.Application.Interfaces;
using ChatApp.API.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatApp.API.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {

        private readonly AuthenticationContext _dbContext;

        public GenericRepository(AuthenticationContext DbContext)
        {
            _dbContext = DbContext;            
        }
        public async Task CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T?> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
        }
    }
}
