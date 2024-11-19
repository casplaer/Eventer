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
        public CategoryRepository(EventsDbContext context) : base(context)
        {
        }

        public async Task<EventCategory?> GetByNameAsync(string name)
        {
            return await _context.Set<EventCategory>().FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
