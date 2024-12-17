using Eventer.Application.Contracts.Enrollments;

namespace Eventer.Application.Interfaces.UseCases.Enrollment
{
    public interface IEnrollOnEventUseCase
    {
        Task ExecuteAsync(EnrollRequest request, Guid userId, CancellationToken cancellationToken);
    }
}
