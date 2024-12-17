using Eventer.Application.Contracts.Events;
using Eventer.Application.Interfaces.UseCases.Events;
using Eventer.Domain.Contracts.Events;
using Eventer.Domain.Interfaces.Repositories;

namespace Eventer.Application.UseCases.Events
{
    public class GetEventByIdUseCase : IGetEventByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEventByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SingleEventResponse?> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var eventToReturn = await _unitOfWork.Events.GetByIdAsync(id, cancellationToken);

            if (eventToReturn == null)
            {
                throw new KeyNotFoundException($"Event with ID '{id}' not found.");
            }

            int regs = eventToReturn.Registrations?.Count ?? 0;

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
    }
}
