using AutoMapper;
using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Domain.Contracts.Enrollments;
using Eventer.Domain.Interfaces.Repositories;

namespace Eventer.Application.UseCases.Enrollment
{
    public class GetSingleEnrollmentByIdUseCase : IGetSingleEnrollmentByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSingleEnrollmentByIdUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SingleEnrollmentResponse?> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.Registrations.GetByIdAsync(id, cancellationToken);
            if (enrollment == null)
            {
                throw new KeyNotFoundException($"Enrollment with ID {id} not found.");
            }

            return _mapper.Map<SingleEnrollmentResponse>(enrollment);
        }
    }

}
