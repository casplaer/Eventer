using Eventer.Application.Interfaces.Repositories;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<EventCategory>, ICategoryRepository
    {
        private readonly EventsDbContext _context;

        public CategoryRepository(EventsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventCategory?>> GetByNameAsync(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return await _context.Categories.ToListAsync();
            }

            return await _context.Categories.Where(c => c.Name.Contains(name)).ToListAsync();
        }
    }
}
