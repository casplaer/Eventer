using Eventer.Application.Exceptions;
using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Domain.Interfaces.Repositories;

namespace Eventer.Application.UseCases.Enrollment
{
    public class DeleteEnrollmentUseCase : IDeleteEnrollmentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEnrollmentUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Registrations.GetByIdAsync(id, cancellationToken);
            if (enrollment == null)
            {
                throw new NotFoundException($"Запись с ID {id} не была найдена.");
            }

            await _unitOfWork.Registrations.DeleteAsync(enrollment, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}
