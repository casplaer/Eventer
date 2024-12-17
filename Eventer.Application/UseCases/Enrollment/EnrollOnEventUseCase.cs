using Eventer.Application.Contracts.Enrollments;
using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Enrollment
{
    public class EnrollOnEventUseCase : IEnrollOnEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<EventRegistration> _validator;
        public EnrollOnEventUseCase(IUnitOfWork unitOfWork,
                IValidator<EventRegistration> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task ExecuteAsync(EnrollRequest request, Guid userId, CancellationToken cancellationToken)
        {
            var userToEnroll = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (userToEnroll == null)
            {
                throw new KeyNotFoundException($"User with ID '{userId}' not found.");
            }

            userToEnroll.EventRegistrations ??= new List<EventRegistration>();

            var registrationToCreate = new EventRegistration
            {
                Name = request.Name,
                Surname = request.SurName,
                Email = request.Email,
                EventId = request.EventId,
                UserId = userId,
                RegistrationDate = DateTime.UtcNow,
                DateOfBirth = request.DateOfBirth
            };

            var validationResult = await _validator.ValidateAsync(registrationToCreate, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var eventToEnrollOn = await _unitOfWork.Events.GetByIdAsync(request.EventId, cancellationToken);
            if (eventToEnrollOn == null)
            {
                throw new KeyNotFoundException($"Event with ID '{request.EventId}' not found.");
            }

            eventToEnrollOn.Registrations.Add(registrationToCreate);
            await _unitOfWork.Events.UpdateAsync(eventToEnrollOn, cancellationToken);

            userToEnroll.EventRegistrations.Add(registrationToCreate);
            await _unitOfWork.Users.UpdateAsync(userToEnroll, cancellationToken);


            await _unitOfWork.Registrations.AddAsync(registrationToCreate, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
