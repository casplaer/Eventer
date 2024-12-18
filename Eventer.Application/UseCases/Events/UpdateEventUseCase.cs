using AutoMapper;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;

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

            var imagePaths = await _imageService.UpdateImagesAsync(
                newImages: request.Images ?? Enumerable.Empty<IFormFile>(),  
                existingImages: request.ExistingImages ?? new List<string>(), 
                removedImages: request.RemovedImages ?? new List<string>(),   
                uploadPath: _uploadPath,
                baseUrl: _httpClient.BaseAddress!.ToString(),
                imageType: "events"
            );

            eventToUpdate.ImageURLs = imagePaths;

            await _validator.ValidateAndThrowAsync(eventToUpdate, cancellationToken);

            await _unitOfWork.Events.UpdateAsync(eventToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
