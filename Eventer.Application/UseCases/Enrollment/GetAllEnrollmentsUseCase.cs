using Eventer.Application.Exceptions;
using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;

namespace Eventer.Application.UseCases.Enrollment
{
    public class GetAllEnrollmentsUseCase : IGetAllEnrollmentsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllEnrollmentsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EventRegistration>> ExecuteAsync(Guid eventId, CancellationToken cancellationToken)
        {
            var eventWithRegistrations = await _unitOfWork.Events.GetByIdAsync(eventId, cancellationToken);

            if (eventWithRegistrations == null)
            {
                throw new NotFoundException($"Event with ID {eventId} not found.");
            }

            return eventWithRegistrations.Registrations;
        }
    }

}
