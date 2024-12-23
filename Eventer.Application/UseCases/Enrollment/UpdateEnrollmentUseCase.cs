using AutoMapper;
using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Contracts.Requests.Enrollments;
using Eventer.Domain.Interfaces.Repositories;
using FluentValidation;

namespace Eventer.Application.UseCases.Enrollment
{
    public class UpdateEnrollmentUseCase : IUpdateEnrollmentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateEnrollRequest> _validator;

        public UpdateEnrollmentUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<UpdateEnrollRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task ExecuteAsync(UpdateEnrollRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var enrollmentToUpdate = await _unitOfWork.Registrations.GetByIdAsync(request.EnrollmentId, cancellationToken);

            _mapper.Map(request, enrollmentToUpdate);

            await _unitOfWork.Registrations.UpdateAsync(enrollmentToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
