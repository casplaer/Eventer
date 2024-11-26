using Eventer.Application.Contracts.Enrollments;
using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.Services
{
    public interface IEnrollmentService
    {
        Task<Guid> IsUserEnrolledAsync(Guid eventId, Guid userId);
        Task<SingleEnrollmentResponse?> GetSingleEnrollmentById(Guid id);
        Task<IEnumerable<EventRegistration>> GetAllEnrollmentsAsync(Guid eventId);
        Task EnrollOnEventAsync(EnrollRequest request, Guid userId);
        Task UpdateEnrollmentAsync(UpdateEnrollRequest request);
        Task<bool> DeleteEnrollmentAsync(Guid id);
    }
}
