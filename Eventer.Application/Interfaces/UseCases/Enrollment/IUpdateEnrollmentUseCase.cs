using Eventer.Domain.Contracts.Enrollments;

namespace Eventer.Application.Interfaces.UseCases.Enrollment
{
    public interface IUpdateEnrollmentUseCase
    {
        Task ExecuteAsync(UpdateEnrollRequest request, CancellationToken cancellationToken);
    }
}
