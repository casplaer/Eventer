using Eventer.Domain.Contracts.Events;

namespace Eventer.Application.Interfaces.UseCases.Events
{
    public interface IAddEventUseCase
    {
        Task ExecuteAsync(CreateEventRequest request, CancellationToken cancellationToken);
    }
}
