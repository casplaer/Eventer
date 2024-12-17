using Eventer.Domain.Models;
using Eventer.Domain.Contracts;
using Eventer.Domain.Contracts.Events;

namespace Eventer.Application.Interfaces.UseCases.Events
{
    public interface IGetUsersEventsUseCase
    {
        Task<PaginatedResult<Event>> ExecuteAsync(UsersEventsRequest request, CancellationToken cancellationToken);
    }
}
