using Eventer.Application.Contracts.Enrollments;
using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Domain.Interfaces.Repositories;

namespace Eventer.Application.UseCases.Enrollment
{
    public class GetSingleEnrollmentByIdUseCase : IGetSingleEnrollmentByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSingleEnrollmentByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SingleEnrollmentResponse?> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Registrations.GetByIdAsync(id, cancellationToken);
            if (enrollment == null)
            {
                throw new KeyNotFoundException($"Enrollment with ID {id} not found.");
            }

            return new SingleEnrollmentResponse(
                enrollment.Id,
                enrollment.EventId,
                enrollment.Name,
                enrollment.Surname,
                enrollment.Email,
                enrollment.DateOfBirth
            );
        }
    }

}
