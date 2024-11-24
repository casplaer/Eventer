using Eventer.Application.Contracts;
using Eventer.Application.Contracts.Events;
using Eventer.Application.Interfaces.Repositories;
using Eventer.Application.Interfaces.Services;
using Eventer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int pageSize = 7;
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

        public async Task<PaginatedResult<Event>> GetFilteredEventsAsync(GetEventsRequest request)
        {
            try
            {
                var eventsToFilter = await _unitOfWork.Events.GetAllAsync();

                EventCategory? tmpCategory = null;

                if (request.CategoryId != null)
                {
                    tmpCategory = (await _unitOfWork.Categories.GetAllAsync()).FirstOrDefault(c => c.Id == request.CategoryId);
                }

                int page = request.Page;

                if (page < 1) page = 1;

                var query = _unitOfWork.Events.GetAllQueryable();

                if (!query.Any())
                {
                    throw new Exception("No events");
                }

                // Фильтрация по названию
                if (!string.IsNullOrEmpty(request.Title))
                {
                    query = query.Where(e => e.Title.Contains(request.Title));
                }

                // Фильтрация по дате
                if (request.Date.HasValue)
                {
                    query = query.Where(e => e.StartDate == request.Date.Value);
                }

                // Фильтрация по месту проведения
                if (!string.IsNullOrEmpty(request.Venue))
                {
                    query = query.Where(e => e.Venue.Contains(request.Venue));
                }

                // Фильтрация по категории
                if (request.CategoryId != null)
                {
                    query = query.Where(e => e.Category == tmpCategory);
                }

                int totalPages = (int)Math.Ceiling((double)query.Count() / pageSize);

                if (totalPages == 0) totalPages = 1;

                query = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                return new PaginatedResult<Event>
                {
                    Items = await query.ToListAsync(),
                    TotalPages = totalPages
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в GetFilteredEventsAsync: {ex.Message}");
                throw;
            }
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
