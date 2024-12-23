using AutoMapper;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Contracts.Requests.Events;
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
        private readonly IMapper _mapper;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "events");

        public AddEventUseCase(
            IUnitOfWork unitOfWork, 
            HttpClient httpClient, 
            IImageService imageService,
            IValidator<Event> validator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _imageService = imageService;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(CreateEventRequest request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.Category.Id, cancellationToken);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {request.Category.Id} not found.");
            }

            var imagePaths = request.Images != null && request.Images.Any()
               ? await _imageService.UploadImagesAsync(request.Images, _uploadPath, _httpClient.BaseAddress!.ToString(), "events")
               : new List<string>();

            var newEvent = _mapper.Map<Event>(request);
            newEvent.Category = category;
            newEvent.ImageURLs = imagePaths;

            await _validator.ValidateAndThrowAsync(newEvent, cancellationToken);

            await _unitOfWork.Events.AddAsync(newEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
