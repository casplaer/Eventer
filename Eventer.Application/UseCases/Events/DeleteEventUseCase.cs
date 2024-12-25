using Eventer.Application.Interfaces.Services;
using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Domain.Interfaces.Repositories;

namespace Eventer.Application.UseCases.Events
{
    public class DeleteEventUseCase : IDeleteEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "events");

        public DeleteEventUseCase(IUnitOfWork unitOfWork,
            IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<bool> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var eventToDelete = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);

            if (eventToDelete == null)
            {
                return false;
            }

            if (eventToDelete.Registrations != null && eventToDelete.Registrations.Any())
            {
                await _unitOfWork.Registrations.RemoveRangeAsync(eventToDelete.Registrations, cancellationToken);
            }

            _imageService.DeleteImages(eventToDelete.ImageURLs, _uploadPath);

            await _unitOfWork.Events.DeleteAsync(eventToDelete, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
