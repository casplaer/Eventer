using Eventer.Application.Contracts;
using Eventer.Application.Contracts.Enrollments;
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
        private readonly string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "events");
        private readonly HttpClient _httpClient;

        public EventService(IUnitOfWork unitOfWork, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _unitOfWork.Events.GetAllAsync();
        }

        public async Task<SingleEventResponse?> GetEventByIdAsync(Guid id)
        {
            var eventToReturn = await _unitOfWork.Events.GetByIdAsync(id);

            if (eventToReturn == null)
            {
                return null;
            }

            int regs = eventToReturn.Registrations.Count;

            return new SingleEventResponse(
                eventToReturn.Id,
                eventToReturn.Title,
                eventToReturn.Description,
                eventToReturn.StartDate,
                eventToReturn.StartTime,
                eventToReturn.Venue,
                eventToReturn.Category,
                eventToReturn.MaxParticipants,
                regs,
                eventToReturn.ImageURLs
            );
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
                return null;
            }
        }


        public async Task AddEventAsync(CreateEventRequest request)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.Category.Id);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {request.Category.Id} not found.");
            }

            var imagePaths = new List<string>();

            if (request.Images != null && request.Images.Any())
            {
                foreach (var image in request.Images)
                {
                    if (image.Length > 0)
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                        var filePath = Path.Combine(uploadPath, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        imagePaths.Add($"{_httpClient.BaseAddress}/uploads/events/{uniqueFileName}");
                    }
                }
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
                ImageURLs = imagePaths
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

            var imagePaths = new List<string>();

            if (request.Images != null && request.Images.Any())
            {
                foreach (var image in request.Images)
                {
                    if (image.Length > 0)
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                        var filePath = Path.Combine(uploadPath, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        imagePaths.Add($"{_httpClient.BaseAddress}/uploads/events/{uniqueFileName}");
                    }
                }
            }

            if (request.ExistingImages != null && request.ExistingImages.Any())
            {
                foreach (var existingImage in request.ExistingImages)
                {
                    if (!string.IsNullOrEmpty(existingImage))
                    {
                        var fileName = Path.GetFileName(existingImage);
                        imagePaths.Add($"{_httpClient.BaseAddress}/uploads/events/{fileName}");
                    }
                }
            }

            if(request.RemovedImages != null && request.RemovedImages.Any())
            {
                foreach(var image in request.RemovedImages)
                {
                    if (image.Length > 0)
                    {
                        var imagePath = Path.Combine(uploadPath, Path.GetFileName(image));
                        File.Delete(imagePath);
                    }
                }
            }

            eventToUpdate.ImageURLs = imagePaths;

            await _unitOfWork.Events.UpdateAsync(eventToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteEventAsync(Guid id)
        {
            var eventToDelete = await _unitOfWork.Events.GetByIdAsync(id);

            if (eventToDelete == null)
                return false;

            var registrationsToDelete = eventToDelete.Registrations;

            if (registrationsToDelete != null && registrationsToDelete.Any())
            {
                foreach (var registration in registrationsToDelete)
                {
                    await _unitOfWork.Registrations.DeleteAsync(registration.Id);
                }
            }


            await _unitOfWork.Events.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        

        

        public async Task<PaginatedResult<Event>> GetUsersEventsAsync(UsersEventsRequest request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

            int page = request.Page;

            if (page < 1) page = 1;

            if (user == null)
            {
                throw new KeyNotFoundException($"Пользователь с ID {request.UserId} не найден.");
            }

            List<Guid> eventIds = [];

            if(user.EventRegistrations != null)
            {
                eventIds = user.EventRegistrations.Select(reg => reg.EventId).Distinct().ToList();
            }

            if (!eventIds.Any())
            {
                return new PaginatedResult<Event> {
                                    Items= new List<Event>(), 
                                    TotalCount = 0, 
                                    TotalPages = 1 
                                    };
            }

            List<Event> usersEvents = [];

            foreach (var eventId in eventIds)
            {
                var eventToAdd = await _unitOfWork.Events.GetByIdAsync(eventId);
                usersEvents.Add(eventToAdd!);
            }

            int totalPages = (int)Math.Ceiling((double)usersEvents.Count() / pageSize);

            if (totalPages == 0) totalPages = 1;

            usersEvents = usersEvents
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<Event>
            {
                Items= usersEvents,
                TotalCount = usersEvents.Count(),
                TotalPages = page,
            };
        }


    }
}
