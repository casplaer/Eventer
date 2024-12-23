using Eventer.Contracts.Responses.Enrollments;

namespace Eventer.Application.Interfaces.UseCases.Enrollment
{
    public interface ICheckUserEnrollmentUseCase
    {
        Task<CheckEnrollmentResponse> ExecuteAsync(Guid eventId, Guid userId, CancellationToken cancellationToken);
    }
}
