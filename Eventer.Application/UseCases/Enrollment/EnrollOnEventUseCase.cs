using AutoMapper;
using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Contracts.Requests.Enrollments;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Enrollment
{
    public class EnrollOnEventUseCase : IEnrollOnEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<EnrollRequest> _validator;
        private readonly IMapper _mapper;

        public EnrollOnEventUseCase(
            IUnitOfWork unitOfWork,
            IValidator<EnrollRequest> validator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(EnrollRequest request, Guid userId, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var userToEnroll = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            var eventToEnrollOn = await _unitOfWork.Events.GetByIdAsync(request.EventId, cancellationToken);

            var registrationToCreate = _mapper.Map<EventRegistration>(request);

            registrationToCreate.UserId = userId;
            registrationToCreate.EventId = request.EventId;
            registrationToCreate.RegistrationDate = DateTime.UtcNow;

            eventToEnrollOn.Registrations.Add(registrationToCreate);
            userToEnroll.EventRegistrations ??= new List<EventRegistration>();
            userToEnroll.EventRegistrations.Add(registrationToCreate);

            await _unitOfWork.Registrations.AddAsync(registrationToCreate, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
