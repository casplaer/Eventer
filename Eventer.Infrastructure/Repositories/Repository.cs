using Eventer.Application.Interfaces.Repositories;
using Eventer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly EventerDbContext _context;

        public Repository(EventerDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

    }
}
