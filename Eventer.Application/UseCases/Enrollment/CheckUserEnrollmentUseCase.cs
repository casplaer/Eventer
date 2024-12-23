using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Contracts.Responses.Enrollments;
using Eventer.Domain.Interfaces.Repositories;

namespace Eventer.Application.UseCases.Enrollment
{
    public class CheckUserEnrollmentUseCase : ICheckUserEnrollmentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckUserEnrollmentUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CheckEnrollmentResponse> ExecuteAsync(Guid eventId, Guid userId, CancellationToken cancellationToken)
        {
            var enrollment = (await _unitOfWork.Registrations.GetAllAsync(cancellationToken))
                .FirstOrDefault(r => r.EventId == eventId && r.UserId == userId);

            if (enrollment == null)
            {
                return new CheckEnrollmentResponse(false, Guid.Empty);
            }

            return new CheckEnrollmentResponse(true, enrollment.Id);
        }
    }

}
