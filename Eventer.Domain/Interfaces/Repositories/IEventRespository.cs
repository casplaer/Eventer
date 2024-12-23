using Eventer.Domain.Contracts;
using Eventer.Domain.Models;

namespace Eventer.Domain.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<PaginatedResult<Event>> GetFilteredEventsAsync(
            string? title,
            DateOnly? date,
            string? venue,
            Guid? categoryId,
            int page,
            CancellationToken cancellationToken);
    }
}
