using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<EventCategory>, ICategoryRepository
    {
        private readonly EventerDbContext _context;

        public CategoryRepository(EventerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventCategory>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .Where(c => c.Name.Contains(name ?? ""))
                .ToListAsync(cancellationToken);
        }
    }
}
