using AutoMapper;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Events
{
    public class UpdateEventUseCase : IUpdateEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly IImageService _imageService;
        private readonly IValidator<Event> _validator;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "events");
        private readonly IMapper _mapper;

        public UpdateEventUseCase(IUnitOfWork unitOfWork, 
            HttpClient httpClient, 
            IMapper mapper,
            IImageService imageService,
            IValidator<Event> validator)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _mapper = mapper;
            _imageService = imageService;
            _validator = validator;
        }

        public async Task ExecuteAsync(UpdateEventRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var eventToUpdate = await _unitOfWork.Events.GetByIdAsync(request.Id, cancellationToken);
            if (eventToUpdate == null)
            {
                throw new KeyNotFoundException($"Event with ID {request.Id} not found.");
            }

            _mapper.Map(request, eventToUpdate);

            if (request.Category != null)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(request.Category.Id, cancellationToken);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {request.Category.Id} not found.");
                }

                eventToUpdate.Category = category;
            }

            var imagePaths = request.Images != null && request.Images.Any()
                    ? await _imageService.UpdateImagesAsync(request.Images, 
                    request.ExistingImages, 
                    request.RemovedImages, _uploadPath, _httpClient.BaseAddress!.ToString(), "events")
                    : new List<string>();

            eventToUpdate.ImageURLs = imagePaths;

            var validationResult = await _validator.ValidateAsync(eventToUpdate, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.Events.UpdateAsync(eventToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
