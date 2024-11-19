using Eventer.Application.Contracts;
using Eventer.Application.Interfaces.Repositories;
using Eventer.Application.Interfaces.Services;
using Eventer.Domain.Models;

namespace Eventer.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _unitOfWork.Events.GetAllAsync();
        }

        public async Task<Event?> GetEventByIdAsync(Guid id)
        {
            return await _unitOfWork.Events.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Event>> GetEventByTitleAsync(string title)
        {
            return await _unitOfWork.Events.GetByTitleAsync(title);
        }

        public async Task<IEnumerable<Event>> GetFilteredEventsAsync(GetEventsRequest request)
        {
            return await _unitOfWork.Events.GetFilteredEventsAsync(request);
        }

        public async Task AddEventAsync(CreateEventRequest request)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.Category.Id);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {request.Category.Id} not found.");
            }

            var newEvent = new Event()
            {
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                StartTime = request.StartTime,
                Venue = request.Venue,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Category = category,
                MaxParticipants = request.MaxParticipants,
            };

            await _unitOfWork.Events.AddAsync(newEvent);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(Guid id)
        {
            await _unitOfWork.Events.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
