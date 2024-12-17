using Eventer.Domain.Contracts;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using Eventer.Infrastructure.Data;
using Eventer.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Infrastructure.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly EventerDbContext _context;
        private readonly int _pageSize = 7;

        public EventRepository(EventerDbContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return result;
        }

        public async Task<PaginatedResult<Event>> GetFilteredEventsAsync(GetEventsRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Events
                        .Include(e => e.Category)
                        .Include(e => e.Registrations)
                        .AsQueryable();

            query = query
                    .ApplyFilter(e => e.Title.Contains(request.Title!), !string.IsNullOrEmpty(request.Title))
                    .ApplyFilter(e => e.StartDate == request.Date!.Value, request.Date.HasValue)
                    .ApplyFilter(e => e.Venue.Contains(request.Venue!), !string.IsNullOrEmpty(request.Venue))
                    .ApplyFilter(e => e.Category.Id == request.CategoryId!.Value, request.CategoryId.HasValue);

            var totalCount = await query.CountAsync(cancellationToken);
            var skip = (Math.Max(1, request.Page) - 1) * _pageSize;
            var items = await query.Skip(skip).Take(_pageSize).ToListAsync(cancellationToken);

            return new PaginatedResult<Event>
            {
                Items = items,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / _pageSize)
            };
        }
    }
}
