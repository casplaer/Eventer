using AutoMapper;
using Eventer.Application.Contracts.Enrollments;
using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Enrollment
{
    public class UpdateEnrollmentUseCase : IUpdateEnrollmentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<EventRegistration> _validator;

        public UpdateEnrollmentUseCase(IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<EventRegistration> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task ExecuteAsync(UpdateEnrollRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var enrollmentToUpdate = await _unitOfWork.Registrations.GetByIdAsync(request.EnrollmentId, cancellationToken);
            if (enrollmentToUpdate == null)
            {
                throw new KeyNotFoundException($"Enrollment with ID {request.EnrollmentId} not found.");
            }

            _mapper.Map(request, enrollmentToUpdate);

            var validationResult = await _validator.ValidateAsync(enrollmentToUpdate, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.Registrations.UpdateAsync(enrollmentToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
