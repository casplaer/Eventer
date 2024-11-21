using Eventer.Application.Contracts.Events;
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

        public async Task UpdateEventAsync(UpdateEventRequest request)
        {
            var eventToUpdate = await _unitOfWork.Events.GetByIdAsync(request.Id);
            if (eventToUpdate == null)
                throw new ArgumentException($"Event with ID {request.Id} not found.");

            if (!string.IsNullOrEmpty(request.Title))
                eventToUpdate.Title = request.Title;

            if (!string.IsNullOrEmpty(request.Description))
                eventToUpdate.Description = request.Description;

            if (request.StartDate.HasValue)
                eventToUpdate.StartDate = request.StartDate.Value;

            if (request.StartTime.HasValue)
                eventToUpdate.StartTime = request.StartTime.Value;

            if (!string.IsNullOrEmpty(request.Venue))
                eventToUpdate.Venue = request.Venue;

            if (request.Latitude.HasValue)
                eventToUpdate.Latitude = request.Latitude.Value;

            if (request.Longitude.HasValue)
                eventToUpdate.Longitude = request.Longitude.Value;
            
            if(request.Category != null)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(request.Category.Id);
                if (category == null)
                    throw new ArgumentException($"Category with ID {request.Category.Id} not found.");

                eventToUpdate.Category = category;
            }

            if (request.MaxParticipants.HasValue)
                eventToUpdate.MaxParticipants = request.MaxParticipants.Value;
            
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteEventAsync(Guid id)
        {
            var eventToDelete = await _unitOfWork.Events.GetByIdAsync(id);

            if (eventToDelete == null)
                return false;
            

            await _unitOfWork.Events.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
