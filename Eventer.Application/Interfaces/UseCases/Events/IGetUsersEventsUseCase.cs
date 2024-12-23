using Eventer.Contracts.Requests.Events;
using Eventer.Domain.Models;
using Eventer.Domain.Contracts;

namespace Eventer.Application.Interfaces.UseCases.Events
{
    public interface IGetUsersEventsUseCase
    {
        Task<PaginatedResult<Event>> ExecuteAsync(UsersEventsRequest request, CancellationToken cancellationToken);
    }
}
