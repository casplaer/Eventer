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
    public class EnrollOnEventUseCase : IEnrollOnEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUniqueFieldChecker _uniqueFieldChecker;

        public EnrollOnEventUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUniqueFieldChecker uniqueFieldChecker)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uniqueFieldChecker = uniqueFieldChecker;
        }

        public async Task ExecuteAsync(EnrollRequest request, Guid userId, CancellationToken cancellationToken)
        {
            if (!await _uniqueFieldChecker.IsUniqueAsync<EventRegistration>("Email", request.Email))
            {
                throw new AlreadyExistsException("Пользователь с таким Email уже зарегистрирован.");
            }

            var userToEnroll = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            var eventToEnrollOn = await _unitOfWork.Events.GetByIdAsync(request.EventId, cancellationToken);

            if (eventToEnrollOn == null)
            {
                throw new NotFoundException("Событие с таким ID не найдено.");
            }

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
