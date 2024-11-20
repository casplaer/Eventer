using Eventer.Application.Contracts;
using Eventer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventer.Application.Interfaces.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(Guid id);
        Task<IEnumerable<Event>> GetEventByTitleAsync(string title);
        Task<IEnumerable<Event>> GetFilteredEventsAsync(GetEventsRequest request);
        Task AddEventAsync(CreateEventRequest request);
        Task UpdateEventAsync(UpdateEventRequest request);
        Task<bool> DeleteEventAsync(Guid id);
    }
}
