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
            return _context.Events.Include(e=>e.Category).FirstOrDefaultAsync(e=>e.Id == id);
        }

        public IQueryable<Event> GetAllQueryable()
        {
            return _context.Events.Include(e => e.Category).AsQueryable();
        }

        public async Task<IEnumerable<Event>> GetFilteredEventsAsync(GetEventsRequest filter)
        {
            /*          EventCategory? tmpCategory = null;

                      if (filter.Category != null && filter.Category.Id != Guid.Empty)
                      {
                          tmpCategory = _context.Categories.FirstOrDefault(c => c.Id == filter.Category.Id);
                      }
                      else if (filter.Category != null && !string.IsNullOrEmpty(filter.Category.Name))
                      {
                          tmpCategory = _context.Categories.FirstOrDefault(c => c.Name == filter.Category.Name);
                      }

                      var query = _context.Events.Include(e => e.Category).AsQueryable();

                      if (!query.Any())
                      {
                          throw new Exception("No events");
                      }

                      //Фильтрация по названию
                      if(!string.IsNullOrEmpty(filter.Title))
                      {
                          query = query.Where(e => e.Title.Contains(filter.Title));
                      }

                      // Фильтрация по дате
                      if (filter.Date.HasValue)
                      {
                          query = query.Where(e => e.StartDate == filter.Date.Value);
                      }

                      // Фильтрация по месту проведения
                      if (!string.IsNullOrEmpty(filter.Venue))
                      {
                          query = query.Where(e => e.Venue.Contains(filter.Venue));
                      }

                      // Фильтрация по категории
                      if (filter.Category != null)
                      {
                          query = query.Where(e => e.Category == tmpCategory);
                      }

                      return await query.ToListAsync();*/
            throw new NotImplementedException();
        }
    }
}
