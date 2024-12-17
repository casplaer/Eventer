using Eventer.Domain.Contracts.Events;

namespace Eventer.Application.Interfaces.UseCases.Events
{
    public interface IUpdateEventUseCase
    {
        Task ExecuteAsync(UpdateEventRequest request, CancellationToken cancellationToken);
    }
}
