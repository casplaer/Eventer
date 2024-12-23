using Eventer.Contracts.Requests.Events;

namespace Eventer.Application.Interfaces.UseCases.Events
{
    public interface IUpdateEventUseCase
    {
        Task ExecuteAsync(UpdateEventRequest request, CancellationToken cancellationToken);
    }
}
