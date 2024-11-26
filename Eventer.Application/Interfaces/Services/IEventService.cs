using Eventer.Application.Contracts;
using Eventer.Application.Contracts.Enrollments;
using Eventer.Application.Contracts.Events;
using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<SingleEventResponse?> GetEventByIdAsync(Guid id);
        Task<PaginatedResult<Event>> GetFilteredEventsAsync(GetEventsRequest request);
        Task<PaginatedResult<Event>> GetUsersEventsAsync(UsersEventsRequest request);
        Task<Guid> IsUserEnrolledAsync(Guid eventId, Guid userId);
        Task<SingleEnrollmentResponse?> GetSingleEnrollmentById(Guid id);
        Task EnrollOnEventAsync(EnrollRequest request, Guid userId);
        Task UpdateEnrollmentAsync(UpdateEnrollRequest request);
        Task<bool> DeleteEnrollmentAsync(Guid id);
        Task AddEventAsync(CreateEventRequest request);
        Task UpdateEventAsync(UpdateEventRequest request);
        Task<bool> DeleteEventAsync(Guid id);
    }
}
