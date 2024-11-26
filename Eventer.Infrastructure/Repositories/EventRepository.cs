using Eventer.Application.Contracts.Events;
using Eventer.Application.Interfaces.Repositories;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Infrastructure.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly EventerDbContext _context;

        public EventRepository(EventerDbContext context) : base(context)
        {
            _context = context;
        }

        public override Task<Event?> GetByIdAsync(Guid id)
        {
            return _context.Events.Include(e=>e.Category)
                        .Include(e => e.Registrations)
                        .FirstOrDefaultAsync(e=>e.Id == id);
        }

        public IQueryable<Event> GetAllQueryable()
        {
            return _context.Events.Include(e => e.Category)
                        .Include(e => e.Registrations)
                        .AsQueryable();
        }

        public async Task<IEnumerable<Event>> GetFilteredEventsAsync(GetEventsRequest filter)
        {
            throw new NotImplementedException();
        }
    }
}
