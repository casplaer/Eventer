using Eventer.Domain.Models;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Contracts;

namespace Eventer.Application.Interfaces.UseCases.Events
{
    public interface IGetFilteredEventsUseCase
    {
        Task<PaginatedResult<Event>> ExecuteAsync(GetEventsRequest request, CancellationToken cancellationToken);
    }
}
