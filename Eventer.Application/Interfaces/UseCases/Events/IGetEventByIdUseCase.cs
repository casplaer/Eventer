using Eventer.Contracts.Responses.Events;

namespace Eventer.Application.Interfaces.UseCases.Events
{
    public interface IGetEventByIdUseCase
    {
        Task<SingleEventResponse?> ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}
