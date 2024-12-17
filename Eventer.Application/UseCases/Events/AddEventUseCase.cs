using Eventer.Application.Interfaces.Services;
using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Events
{
    public class AddEventUseCase : IAddEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly IImageService _imageService;
        private readonly IValidator<Event> _validator;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "events");

        public AddEventUseCase(IUnitOfWork unitOfWork, 
                HttpClient httpClient, 
                IImageService imageService,
                IValidator<Event> validator)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _imageService = imageService;
            _validator = validator;
        }

        public async Task ExecuteAsync(CreateEventRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(request.Category.Id, cancellationToken);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {request.Category.Id} not found.");
            }

            var imagePaths = request.Images != null && request.Images.Any()
               ? await _imageService.UploadImagesAsync(request.Images, _uploadPath, _httpClient.BaseAddress!.ToString(), "events")
               : new List<string>();

            var newEvent = new Event
            {
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                StartTime = request.StartTime,
                Venue = request.Venue,
                Category = category,
                MaxParticipants = request.MaxParticipants,
                ImageURLs = imagePaths
            };

            var validationResult = await _validator.ValidateAsync(newEvent, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.Events.AddAsync(newEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
