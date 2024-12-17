using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.UseCases.Enrollment
{
    public interface IGetAllEnrollmentsUseCase
    {
        Task<IEnumerable<EventRegistration>> ExecuteAsync(Guid eventId, CancellationToken cancellationToken);
    }

}
