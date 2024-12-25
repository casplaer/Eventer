using AutoMapper;
using Eventer.Application.Exceptions;
using Eventer.Application.Interfaces;
using Eventer.Application.Interfaces.Services;
using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Contracts.Requests.Events;
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
        private readonly IUniqueFieldChecker _uniqueFieldChecker;

        public UpdateEventUseCase(IUnitOfWork unitOfWork, 
            HttpClient httpClient, 
            IMapper mapper,
            IImageService imageService,
            IValidator<Event> validator,
            IUniqueFieldChecker uniqueFieldChecker)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _mapper = mapper;
            _imageService = imageService;
            _validator = validator;
            _uniqueFieldChecker = uniqueFieldChecker;
        }

        public async Task ExecuteAsync(UpdateEventRequest request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _unitOfWork.Events.GetByIdAsync(request.Id, cancellationToken);
            if (eventToUpdate == null)
            {
                throw new NotFoundException($"Событие с ID {request.Id} не найдено.");
            }

            if (!await _uniqueFieldChecker.IsUniqueAsync<Event>("Title", request.Title, request.Id))
            {
                throw new AlreadyExistsException("Событие с таким названием уже существует.");
            }

            _mapper.Map(request, eventToUpdate);

            if (request.Category != null)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(request.Category.Id, cancellationToken);
                if (category == null)
                {
                    throw new NotFoundException($"Категория с ID {request.Category.Id} не найдена.");
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
