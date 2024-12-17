using Eventer.Domain.Contracts;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Models;

namespace Eventer.Domain.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<PaginatedResult<Event>> GetFilteredEventsAsync(GetEventsRequest request, CancellationToken cancellationToken);
    }
}
