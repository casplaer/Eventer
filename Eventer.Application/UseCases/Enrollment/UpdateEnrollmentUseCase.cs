using AutoMapper;
using Eventer.Application.Exceptions;
using Eventer.Application.Interfaces;
using Eventer.Application.Interfaces.UseCases.Enrollment;
using Eventer.Contracts.Requests.Enrollments;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Application.UseCases.Enrollment
{
    public class UpdateEnrollmentUseCase : IUpdateEnrollmentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUniqueFieldChecker _uniqueFieldChecker;

        public UpdateEnrollmentUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUniqueFieldChecker uniqueFieldChecker)
        {
            _uniqueFieldChecker = uniqueFieldChecker;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(UpdateEnrollRequest request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Registrations.GetByIdAsync(request.EnrollmentId, cancellationToken) == null)
            {
                throw new NotFoundException("Регистрация с таким ID не найдена.");
            }

            if (!await _uniqueFieldChecker.IsUniqueAsync<EventRegistration>("Email", request.Email,
                    request.EnrollmentId))
            {
                throw new AlreadyExistsException("Пользователь с таким Email уже зарегистрирован.");
            }

            var enrollmentToUpdate = await _unitOfWork.Registrations.GetByIdAsync(request.EnrollmentId, cancellationToken);

            _mapper.Map(request, enrollmentToUpdate);

            await _unitOfWork.Registrations.UpdateAsync(enrollmentToUpdate, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
