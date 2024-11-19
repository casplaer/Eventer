using Eventer.Application.Interfaces.Repositories;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;

namespace Eventer.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventsDbContext _context;
        private IEventRepository? _eventRepository;
        private ICategoryRepository? _categoryRepository;

        public UnitOfWork(EventsDbContext context)
        {
            _context = context;
        }

        public IEventRepository Events => _eventRepository ??= new EventRepository(_context);
        public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
