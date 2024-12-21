using Eventer.Application.Interfaces;
using Eventer.Domain.Contracts.Enrollments;
using Eventer.Domain.Interfaces.Repositories;
using Eventer.Domain.Models;
using FluentValidation;

namespace Eventer.Infrastructure.Validators.Enrollments
{
    public class EnrollRequestValidator : AbstractValidator<EnrollRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUniqueFieldChecker _uniqueFieldChecker;

        public EnrollRequestValidator(IUnitOfWork unitOfWork, IUniqueFieldChecker uniqueFieldChecker)
        {
            _unitOfWork = unitOfWork;
            _uniqueFieldChecker = uniqueFieldChecker;

            RuleFor(req => req.Name)
                .NotEmpty().WithMessage("Имя обязательно.")
                .MaximumLength(100).WithMessage("Имя не может превышать 100 символов.");

            RuleFor(req => req.SurName)
                .NotEmpty().WithMessage("Фамилия обязательна.")
                .MaximumLength(100).WithMessage("Фамилия не может превышать 100 символов.");

            RuleFor(req => req.Email)
                .NotEmpty().WithMessage("Поле Email обязательно.")
                .MaximumLength(255).WithMessage("Поле Email не может превышать 255 символов.")
                .EmailAddress().WithMessage("Некорректный формат Email.")
                .MustAsync(async (req, email, cancellation) =>
                    await _uniqueFieldChecker.IsUniqueAsync<EventRegistration>("Email", email))
                .WithMessage("Пользователь с таким Email уже зарегистрирован.");

            RuleFor(req => req.DateOfBirth)
                .NotEmpty().WithMessage("Дата рождения обязательна.");

            RuleFor(req => req.EventId)
                .MustAsync(async (eventId, cancellation) =>
                    await _unitOfWork.Events.GetByIdAsync(eventId, cancellation) != null)
                .WithMessage("Событие с указанным ID не найдено.");
        }
    }
}
