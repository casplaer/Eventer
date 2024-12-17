using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.UseCases.Events
{
    public interface IGetAllEventsUseCase
    {
        Task<IEnumerable<Event>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
