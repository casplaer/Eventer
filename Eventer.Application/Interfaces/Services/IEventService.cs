using Eventer.Application.Contracts.Events;
using Eventer.Domain.Models;

namespace Eventer.Application.Interfaces.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(Guid id);
        Task<IEnumerable<Event>> GetFilteredEventsAsync(GetEventsRequest request);
        Task AddEventAsync(CreateEventRequest request);
        Task UpdateEventAsync(UpdateEventRequest request);
        Task<bool> DeleteEventAsync(Guid id);
    }
}
